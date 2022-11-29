/*
 * Zombie.cs
 * 
 * Controls the main enemies, zombies; which walk toward the player, and damage the player 
 * if touched. The boss enemy also uses this script.
 * 
 * This script is attatched to all zombies.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;
using System;

public class Zombie : MonoBehaviour
{
    public bool hitByPlayer = false;

    [SerializeField]
    private float _speed = 2.5f;
    [SerializeField]
    private float _minimumY = -10f;
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
    [SerializeField]
    private AudioSource _moanSound;
    [SerializeField]
    private AudioSource _dieSound;

    private float _dir = 0;
    private float wallDirection;
    private GameObject _hero;
    private Camera _cam;
    private bool dead = false;
    private bool hitWall;
    private Animator zomAnim;

    public static bool move = true;

    // Start is called before the first frame update
    void Start()
    {
        _hero = GameObject.FindWithTag("Player");
        _cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        StartCoroutine("Moan");
        zomAnim = this.GetComponent<Animator>();
    }

    void HitWall()
    {
        Vector3 position1 = new Vector3(this.transform.position.x, this.transform.position.y + .5f, this.transform.position.z);
        Vector3 position2 = new Vector3(this.transform.position.x, this.transform.position.y + 2.3f, this.transform.position.z);
        Vector3 position3 = new Vector3(this.transform.position.x, this.transform.position.y + 1.4f, this.transform.position.z);
        //Debug.DrawRay(position1, this.transform.forward, Color.green);
        //Debug.DrawRay(position2, this.transform.forward, Color.green);
        //Debug.DrawRay(position3, this.transform.forward, Color.green);
        RaycastHit hitSideF;
        Ray raySideF = new Ray(position1, transform.forward);

        if (Physics.Raycast(raySideF, out hitSideF))
        {
            if (hitSideF.transform.gameObject.tag == "ground")//&& hitSideF.transform.rotation.z == 0)
            {
                if (hitSideF.distance <= 0.6)
                {
                    hitWall = true;
                    //Debug.Log(hitSideF.distance);
                }
                else
                {
                    hitWall = false;
                    //Debug.Log("Wasn't close enough");
                }

            }
            else
            {
                hitWall = false;
                //Debug.Log("Didn't hit ground.");
            }
        }

        if (!hitWall)
        {
            raySideF = new Ray(position2, transform.forward);
            if (Physics.Raycast(raySideF, out hitSideF))
            {
                if (hitSideF.transform.gameObject.tag == "ground")//&& hitSideF.transform.rotation.z == 0)
                {
                    if (hitSideF.distance <= .6)
                    {
                        hitWall = true;
                    }
                    else
                    {
                        hitWall = false;
                    }
                }
            }
        }

        if (!hitWall)
        {
            raySideF = new Ray(position3, transform.forward);
            if (Physics.Raycast(raySideF, out hitSideF))
            {
                if (hitSideF.transform.gameObject.tag == "ground")//&& hitSideF.transform.rotation.z == 0)
                {
                    if (hitSideF.distance <= .6)
                    {
                        hitWall = true;
                    }
                    else
                    {
                        hitWall = false;
                    }
                }
            }
        }
    }
    IEnumerator Moan()
    {
        while(!dead)
        {
            Vector3 pos = transform.position;
            float maxMovDist = _hero.transform.position.x + _cam.orthographicSize + _moveOffset;
            float minMovDist = _hero.transform.position.x - _cam.orthographicSize - _moveOffset;
            if (_boss || (move && pos.x > minMovDist && pos.x < maxMovDist))
            {
                if (!(_boss && !BossFight.fightStarted))
                {
                    yield return new WaitForSeconds(Range(0.1f, 0.3f));
                    if(!dead)
                        _moanSound.Play();
                    yield return new WaitForSeconds(Range(1.5f, 4));
                }
                else
                    yield return null;
            }
            else
                yield return null;
        }
    }

    void Update()
    {
        // If this is the boss, and the boss fight hasn't started, return.
        if (dead || (_boss && !BossFight.fightStarted))
            return;

        if (transform.position.y < _minimumY)
        {
            Destroy(this.gameObject);
        }

        Vector3 pos = transform.position;
        float maxMovDist = _hero.transform.position.x + _cam.orthographicSize + _moveOffset;
        float minMovDist = _hero.transform.position.x - _cam.orthographicSize - _moveOffset;

        // move the zombie, if it should be moved.
        if (_boss || (move && pos.x > minMovDist && pos.x < maxMovDist))
        {
            zomAnim.SetFloat("speed", _speed);
            // Get the horizontal distance from the player
            float horizontal = pos.x - _hero.transform.position.x;
            //Debug.Log(horizontal);
            if(Math.Abs(horizontal)<1)
            {
                zomAnim.SetBool("attack", true);
                Invoke("resetAttackBool", .5f);
            }   
            // choose movement direction
            if (horizontal < -0.1)
            {
                _dir = 1;
                transform.rotation = Quaternion.Euler(0, 90, 0);
 
            }
            else if (horizontal > 0.1)
            {
                _dir = -1;
                transform.rotation = Quaternion.Euler(0, -90, 0);
            }
            else
            {
                return;
            }

            // move toward player
            HitWall();
            if(!hitWall)
            {
                pos.x += (_dir * Time.deltaTime * _speed);
                transform.position = pos;
            }
            else
            {
                zomAnim.SetFloat("speed", 0);
            }

        }
    }
    
    void resetAttackBool()
    {
        zomAnim.SetBool("attack", false);
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
            dead = true;
            _dieSound.Play();
            if (hitByPlayer)
                _cam.gameObject.GetComponent<Game>().UpdateScore(_killScore);

            StartCoroutine("Die");
        }
        else
            hitByPlayer = false;
    }

    // Keep the zombie hidden (but still existant) long enough to play the death sound,
    // then destroy it.
    IEnumerator Die()
    {
        this.gameObject.layer = 7;
        this.gameObject.tag = "Player";
        zomAnim.SetBool("death", true);
        this.GetComponent<BoxCollider>().enabled = true;
        this.GetComponent<MeshCollider>().enabled = false;
        Invoke("moveZombieAfterDeath", 2f);
        yield return new WaitForSeconds(2f);

        Destroy(this.gameObject);
    }

    void moveZombieAfterDeath()
    {
        Vector3 pos = transform.position;
        pos.y = -100;
        transform.position = pos;
    }
}