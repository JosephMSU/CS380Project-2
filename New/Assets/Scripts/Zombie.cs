/*
 * Zombie.cs
 * Main Author:  Jason
 * Other Authors: 
 * 
 * Controls the main enemies, zombies; which walk toward the player, and damage the player 
 * if touched.
 * 
 * This script is attatched to all zombies.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public bool hitByPlayer = false;

    [SerializeField]
    private float _speed = 2.5f;
    [SerializeField]
    private int _killScore = 1;
    [SerializeField]
    private int _dmgAmt = 1;
    [SerializeField]
    private int _health = 1;
    [SerializeField]
    private float _moveOffset = 4.8f;
    [SerializeField]
    private bool _boss = false;

    private float _dir = 0;
    private GameObject _hero;
    private Camera _cam;

    public static bool move = true;

    // Start is called before the first frame update
    void Start()
    {
        _hero = GameObject.FindWithTag("Player");
        _cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        // If this is the boss, and the boss fight hasn't started, return.
        if (_boss && !BossFight.fightStarted)
            return;

        Vector3 pos = transform.position;
        float maxMovDist = _hero.transform.position.x + _cam.orthographicSize + _moveOffset;
        float minMovDist = _hero.transform.position.x - _cam.orthographicSize - _moveOffset;

        // move the zombie, if it should be moved.
        if (_boss || (move && pos.x > minMovDist && pos.x < maxMovDist))
        {

            // Get the horizontal distance from the player
            float horizontal = pos.x - _hero.transform.position.x;

            // choose movement direction
            if (horizontal < -0.1)
            {
                _dir = 1;
                transform.rotation = Quaternion.Euler(-90, 90, 0);
            }
            else if (horizontal > 0.1)
            {
                _dir = -1;
                transform.rotation = Quaternion.Euler(-90, -90, 0);
            }
            else
            {
                return;
            }

            // move toward player
            pos.x += (_dir * Time.deltaTime * _speed);
            transform.position = pos;
        }
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject == _hero && !Player.invincible)
        {
            // damage the player
            print("Collided With Hero");
            _hero.GetComponent<Player>().TakeDamage(_dmgAmt);
        }
    }

    public void TakeDamage(int amtOfDmg)
    {
        _health -= amtOfDmg;
        if (_boss)
            _cam.gameObject.GetComponent<BossFight>().BossHit();

        if (_health <= 0)
        {
            if (hitByPlayer)
                _cam.gameObject.GetComponent<Game>().UpdateScore(_killScore);

            Destroy(this.gameObject);
        }
        else
            hitByPlayer = false;
    }
}