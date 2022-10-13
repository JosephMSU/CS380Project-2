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


    private Vector3 _camPos;
    private Vector3 _oldPos;


    // Start is called before the first frame update
    void Start()
    {
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
        Vector3 heroPos = _hero.transform.position;
        Vector3 newPos;
        newPos.x = heroPos.x + _xOffset;
        newPos.y = heroPos.y + _yOffset;
        newPos.z = -(_zOffset);

        _camPos = Vector3.Lerp(_oldPos, newPos, 0.05f);
        _oldPos = _camPos;
        transform.position = _camPos;
    }
}
