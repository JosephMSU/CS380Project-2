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
    private GameObject _hero;
    private Camera _cam;
    private bool dead = false;

    public static bool move = true;

    // Start is called before the first frame update
    void Start()
    {
        _hero = GameObject.FindWithTag("Player");
        _cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        StartCoroutine("Moan");
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
        Vector3 pos = transform.position;
        pos.y = -100;
        transform.position = pos;

        yield return new WaitForSeconds(2f);

        Destroy(this.gameObject);
    }
}