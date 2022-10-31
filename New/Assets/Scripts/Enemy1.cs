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
    [SerializeField]
    private int _health = 1;
    [SerializeField]
    private Camera cam;

    private float _dir = 0;
    public static bool move = true;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        float maxMovDist = cam.transform.position.x + cam.orthographicSize + 4;
        float minMovDist = cam.transform.position.x - cam.orthographicSize - 4;

        // move the zombie, if it should be moved.
        if (move && pos.x > minMovDist && pos.x < maxMovDist)
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
            //_hero.GetComponent<Player>().TakeDamage(_dmgAmt);
        }
    }

    public void TakeDamage()
    {
        _health--;
        if (_health == 0)
        {
            Destroy(this.gameObject);
        }
    }
}
