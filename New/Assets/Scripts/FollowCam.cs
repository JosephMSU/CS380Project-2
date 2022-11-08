/*
 * FollowCam.cs
 * Main Author:  Jason
 * Other Authors: 
 * 
 * Controls the camera, by making it follow the player.
 * 
 * This script is attached to the cameras in the level scenes.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField]
    private GameObject _hero;
    [SerializeField]
    private float _xOffset = 1.7f;
    [SerializeField]
    private float _yOffset = 1f;
    [SerializeField]
    private float _zOffset = 10;
    [SerializeField]
    private float _leftBound = -10;
    [SerializeField]
    private float _rightBound = 200;
    [SerializeField]
    private float _interpolateAmt = 0.2f;

    private Vector3 _camPos;
    private Vector3 _oldPos;

    [HideInInspector]
    public bool follow = true;

    // Start is called before the first frame update
    void Start()
    {
        // set the camera's inital position
        _camPos = _hero.transform.position;
        _camPos.x += _xOffset;
        _camPos.y += _yOffset;
        _camPos.z -= _zOffset;
        transform.position = _camPos;
        _oldPos = _camPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (!follow)
            return;

        // set the camera's new position
        Vector3 heroPos = _hero.transform.position;
        Vector3 newPos;
        newPos.x = heroPos.x + _xOffset;
        newPos.y = heroPos.y + _yOffset;
        newPos.z = -(_zOffset);

        // Interpolate between the old and new position for smoother camera movement
        _camPos = Vector3.Lerp(_oldPos, newPos, _interpolateAmt);
        _oldPos = _camPos;

        // keep the camera within bounds
        if (_camPos.x < _leftBound)
            _camPos.x = _leftBound;
        else if (_camPos.x > _rightBound)
            _camPos.x = _rightBound;

        // set the camera's position
        transform.position = _camPos;
    }

    private float _zoomSpd;
    public void ZoomBackToPlayer(float speed)
    {
        _zoomSpd = speed;
        StartCoroutine("ZoomToPlayer");
    }

    IEnumerator ZoomToPlayer()
    {
        // set the camera's new position
        Vector3 heroPos = _hero.transform.position;
        Vector3 newPos;
        newPos.x = heroPos.x + _xOffset;
        newPos.y = heroPos.y + _yOffset;
        newPos.z = -(_zOffset);

        while(transform.position.y != newPos.y ||
              transform.position.x != newPos.x ||
              GetComponent<Camera>().orthographicSize > 6)
        {
            if (GetComponent<Camera>().orthographicSize > 6)
                GetComponent<Camera>().orthographicSize -= _zoomSpd * 0.275f * Time.deltaTime;
            else
                GetComponent<Camera>().orthographicSize = 6;

            Vector3 pos = Vector3.MoveTowards(transform.position, newPos, _zoomSpd * Time.deltaTime);

            transform.position = pos;

            yield return null;
        }
        _oldPos = newPos;
        _camPos = newPos;
        follow = true;
        _hero.GetComponent<Player>().doNotMove = false;
    }
}