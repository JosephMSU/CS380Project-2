using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField]
    private GameObject _hero;
    [SerializeField]
    private float _zOffset = 10;

    private Vector3 _camPos;


    // Start is called before the first frame update
    void Start()
    {
        _camPos = _hero.transform.position;
        _camPos.z -= _zOffset;
        transform.position = _camPos;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _heroPos = _hero.transform.position;
        _camPos.x = _heroPos.x;
        _camPos.y = _heroPos.y;
        transform.position = _camPos;
    }
}
