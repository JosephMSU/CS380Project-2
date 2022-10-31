using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Added for interfacing with the enemy script. Feel free to change the name or
    // suggest a different method.  - Jason (remove comment before release)
    [HideInInspector]
    public static bool invincible;
    private Rigidbody rBody;
    private MeshCollider mesh;

    [SerializeField]
    private float speed = 5f;
    public float jumpSpeed = 10f;

    [SerializeField]
    private GameObject Projectile;

    private bool left = false;
    private float dmgMult; // Damage multiplier, changes damage taken from enemies based on difficulty level. - Jason
                   // (remove comment before release)

    // Start is called before the first frame update
    void Start()
    {
        rBody = transform.GetComponent<Rigidbody>();
        mesh = transform.GetComponent<MeshCollider>();
        dmgMult = PlayerPrefs.GetFloat("difficulty"); // I am using PlayerPrefs to store difficulty setting
                                                      // between plays. - Jason (remove comment before release)
    }

    // Update is called once per frame
    void Update()
    {
        /*float x = this.transform.position.x - .4f;
        float x2 = this.transform.position.x + .4f;
        Vector3 position4 = new Vector3(x, this.transform.position.y, this.transform.position.z);
        Vector3 position5 = new Vector3(x2, this.transform.position.y, this.transform.position.z);
        Vector3 down = transform.TransformDirection(Vector3.down) * 10;
        Debug.DrawRay(transform.position, down, Color.green);
        Debug.DrawRay(position4, down, Color.red);
        Debug.DrawRay(position5, down, Color.blue);*/
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (horizontal > 0)
        {
            left = false;
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (horizontal < 0)
        {
            left = true;
            transform.rotation = Quaternion.Euler(0, -90, 0);

        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            shoot();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            jump();
        }

        Vector3 pos = transform.position;
        pos.x += (horizontal * Time.deltaTime * speed);
        transform.position = pos;


    }

    // The player takes damage (added to interface with Enemy class.  I wanted to make it possible for
    // different enemys to give the player different amounts of damage if we decide to do so, and wanted to
    // make sure you are aware of it by adding this.  Feel free to change the name of the function, and I will
    // update the Enemy script to match, or you can suggest a different approach.)  -Jason
    // (remove comment before release)
    /*public void TakeDamage(int dmgAmt)
    {
        int totalDamage = (int)(dmgAmt * dmgMult);
        if (totalDamage == 0)
            totalDamage = 1;
    }*/

    public void jump()
    {
        if (isGrounded())
        {
            rBody.velocity = Vector3.up * jumpSpeed;
        }
    }

    public void shoot()
    {
        // set the position of the proectile
        Vector3 projectilePos = this.transform.position;
        projectilePos.y += 2;
        int projectileDirection;
        if (left)
        {
            projectileDirection = -1;
            //projectilePos.x -= 1;
        }
        else
        {
            projectileDirection = 1;
            //projectilePos.x += 1;
        }

        // Create the projectile, and set it's movement direction
        GameObject proj = Instantiate(Projectile, projectilePos , Quaternion.identity);
        proj.GetComponent<Projectile>().dir = projectileDirection;
    }

    public bool isGrounded()
    {
        bool grounded = false;
        RaycastHit hit1;
        Vector3 position1 = new Vector3(this.transform.position.x, this.transform.position.y + .5f, this.transform.position.z);
        Ray ray1 = new Ray(position1, (this.transform.up * -1));
        Vector3 position2 = new Vector3(this.transform.position.x - .4f, this.transform.position.y+.5f, this.transform.position.z);
        Vector3 position3 = new Vector3(this.transform.position.x + .4f, this.transform.position.y+.5f, this.transform.position.z);
        RaycastHit hit2;
        Ray ray2 = new Ray(this.transform.position, (this.transform.up * -1));
        RaycastHit hit3;
        Ray ray3 = new Ray(this.transform.position, (this.transform.up * -1));
        /*Physics.Raycast(ray2, out hit2);
        Physics.Raycast(ray3, out hit3);
        Physics.Raycast(ray1, out hit1);
        Debug.DrawRay(position2, this.transform.up * -1, Color.green);*/
        if(Physics.Raycast(ray1, out hit1))
        {
            if(hit1.transform.tag =="ground")
            {
                Debug.Log("Hit the ground");
                Debug.Log(hit1.distance);
                if (hit1.distance <= .5)
                {
                    grounded = true;
                    Debug.Log("Distance = 0");
                }
            }
        }
        else if(Physics.Raycast(ray2, out hit2))
        {
            if(hit2.transform.gameObject.tag == "ground")
            {
                if(hit2.distance <= .5)
                {
                    grounded = true;
                }
            }
        }
        else if(Physics.Raycast(ray3, out hit3))
        {
            if(hit3.transform.gameObject.tag == "ground")
            {
                if(hit3.distance <= .5)
                {
                    grounded = true;
                }
            }
        }
        else
        {
            Debug.Log("Didn't hit");
        }


        /*float hitDistance1 = hit1.distance;
        float hitDistance2 = hit2.distance;
        float hitDistance3 = hit3.distance;
        Debug.Log(hitDistance1);*/
        return grounded;
        //return (hitDistance1 == 0 || hitDistance2 == 0 || hitDistance3 == 0);
    }
}
