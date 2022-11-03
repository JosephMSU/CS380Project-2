/*
 * FollowCam.cs
 * Main Author:  Jason
 * Other Authors: 
 * 
 * Controls the movement of moving platforms.  Assumes pos1's x position is either to the
 * left of or the same as pos2's x position.
 * 
 * This script is attached to moving platforms.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlaform : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private Vector3 _pos1;
    [SerializeField]
    private Vector3 _pos2;
    [SerializeField]
    private float _startOffset = 12;
    [SerializeField]
    private bool _overrideEasyDifficultySlowDown = false;
    [SerializeField]
    private bool _overrideHardDifficultySpeedUp = false;

    private bool _toPos1;
    private bool _started;
    private GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        float difficulty = PlayerPrefs.GetFloat("difficulty");
        if (difficulty == 0.5f && _overrideEasyDifficultySlowDown)
            return;
        else if (difficulty == 2 && _overrideHardDifficultySpeedUp)
            return;
        else
            _speed *= PlayerPrefs.GetFloat("difficulty");
    }

    void Update()
    {
        if (_player.transform.position.x + _startOffset >= transform.position.x && !_started)
        {
            StartCoroutine("ToPos1");
            _started = true;
        }
    }

    IEnumerator ToPos1()
    {
        _toPos1 = true;
        while (transform.localPosition != _pos1)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _pos1, _speed * Time.deltaTime);
            yield return null;
        }

        StartCoroutine("ToPos2");
    }

    IEnumerator ToPos2()
    {
        _toPos1 = false;
        while (transform.localPosition != _pos2)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _pos2, _speed * Time.deltaTime);
            yield return null;
        }

        StartCoroutine("ToPos1");
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "zombie")
        {
            if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Player>().IsGrounded() == false)
                return;

            // Move the other object with the platform
            Vector3 movingTo;
            if (_toPos1)
                movingTo = _pos1;
            else
                movingTo = _pos2;

            Vector3 newPos = Vector3.MoveTowards(transform.localPosition, movingTo, _speed * Time.deltaTime);
            Vector3 movObjAmt = newPos - transform.localPosition;
            movObjAmt.y -= 0.1f;

            other.transform.position += movObjAmt;
        }
    }
}
