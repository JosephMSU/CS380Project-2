/*
 * DamagingObstacle.cs
 * 
 * This script controls non-moving obstacles that damages the player and (optionally) 
 * enemies. All obstacles in the intital release will damage both the player and the 
 * enemies. The ability to make bstacles that damage the player, but not enemies, was 
 * added to this script in case we want to implement it in a future update after the
 * initial release.
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
            Debug.Log("Collided With Hero");
            _hero.GetComponent<Player>().TakeDamage(_dmgAmt);
        }
        else if (other.gameObject.tag == "zombie" && _damagesZombies)
        {
            // damage the zombie
            Debug.Log("Collided With Enemy");
            other.gameObject.GetComponent<Zombie>().TakeDamage(_dmgAmt);
        }
    }
}