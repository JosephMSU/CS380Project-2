/*
 * DamagingObstacle.cs
 * Main Author:  Jason
 * Other Authors: 
 * 
 * Controls non-moving obstacles that damages the player and (optionally) enemies.
 * 
 * This script is attached to some, but not all, obstacles.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingObsacle : MonoBehaviour
{
    [SerializeField]
    private int _dmgAmt = 1;
    [SerializeField]
    private bool _damagesZombies = true;

    private GameObject _hero;

    // Start is called before the first frame update
    void Start()
    {
        _hero = GameObject.FindWithTag("Player");
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject == _hero && !Player.invincible)
        {
            // damage the player
            print("Collided With Hero");
            //_hero.GetComponent<Player>().TakeDamage(_dmgAmt);
        }
        else if (other.gameObject.tag == "zombie" && _damagesZombies)
        {
            // damage the zombie
            print("Collided With Enemy");
            other.gameObject.GetComponent<Enemy1>().TakeDamage(_dmgAmt);
        }
    }
}