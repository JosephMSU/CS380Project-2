/*
 * Projectile.cs
 * Main Author:  Jason
 * Other Authors:  
 * 
 * Manages projectiles shot by the player.
 *     
 * This script is attached all player projectiles.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float _speed = 20f;
    [SerializeField]
    private int _dmgAmt = 1;

    [HideInInspector]
    public int dir; // -1 for left, or 1 for right.  Public because the Player script needs to set it's value.
    private Camera cam;

    float maxDist;
    float minDist;
    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        // set the position
        Vector3 pos = transform.position;
        pos.x += (dir * Time.deltaTime * _speed);
        transform.position = pos;

        if (BossFight.fightStarted)
            return;

        // destroy if outside of camera
        float maxDist = cam.transform.position.x + cam.orthographicSize + 4;
        float minDist = cam.transform.position.x - cam.orthographicSize - 4;
        if (pos.x > maxDist || pos.x < minDist)
            Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
            return;

        if (other.gameObject.tag == "zombie" || other.gameObject.tag == "boss")
        {
            other.gameObject.GetComponent<Zombie>().hitByPlayer = true;
            other.gameObject.GetComponent<Zombie>().TakeDamage(_dmgAmt);
        }

        Destroy(this.gameObject);
    }
}