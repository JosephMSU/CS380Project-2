using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField]
    private GameObject _hero;
    [SerializeField]
    private float _xOffset = 0;
    [SerializeField]
    private float _yOffset = 2.5f;
    [SerializeField]
    private float _zOffset = 10;
    [SerializeField]
    private float _leftBound = -10;
    [SerializeField]
    private float _rightBound = 100;


    private Vector3 _camPos;
    private Vector3 _oldPos;
    bool _follow = true;


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
        if (!_follow)
            return;

        // set the camera's new position
        Vector3 heroPos = _hero.transform.position;
        Vector3 newPos;
        newPos.x = heroPos.x + _xOffset;
        newPos.y = heroPos.y + _yOffset;
        newPos.z = -(_zOffset);

        // Interpolate between the old and new position for smoother camera movement
        _camPos = Vector3.Lerp(_oldPos, newPos, 0.05f);
        _oldPos = _camPos;

        // keep the camera within bounds
        if (_camPos.x < _leftBound)
            _camPos.x = _leftBound;
        else if (_camPos.x > _rightBound)
            _camPos.x = _rightBound;

        // set the camera's position
        transform.position = _camPos;
    }
}
