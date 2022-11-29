/*
 * Projectile.cs 
 * 
 * Manages projectiles shot by the player. Projectiles damage any enemies they touch.
 * This script will also destroy projectiles that hit the ground or an enemy, or travels
 * off the screen.
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

    private float maxDist;
    private float minDist;

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
        float maxDist = cam.transform.position.x + cam.orthographicSize-3;
        float minDist = cam.transform.position.x - cam.orthographicSize - 12f;
        //Debug.Log(maxDist);
        //Debug.Log(minDist);
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
            //Debug.Log("Hit Zombie");
        }

        Destroy(this.gameObject);
    }
}