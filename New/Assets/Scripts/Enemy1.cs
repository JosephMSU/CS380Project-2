using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2.5f;
    [SerializeField]
    private GameObject _hero;
    [SerializeField]
    private int _dmgAmt = 1;

    private float _dir = 0;
    public static bool move = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            Vector3 pos = transform.position;
            float horizontal = pos.x - _hero.transform.position.x;
            float mov;

            if (horizontal < -0.1)
            {
                _dir = 1;
            }
            else if (horizontal > 0.1)
            {
                _dir = -1;
            }
            else
            {
                return;
            }

            pos.x += (_dir * Time.deltaTime * _speed);
            transform.position = pos;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        /*if (other.gameObject == _hero && !Player.invincible)
        {
            _hero.GetComponent<Player>().TakeDamage(_dmgAmt);
        }*/
    }
}
