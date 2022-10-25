using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Added for interfacing with the enemy script. Feel free to change the name or
    // suggest a different method.  - Jason
    [HideInInspector]
    public static bool invincible;
    private Rigidbody rBody;
    private MeshCollider mesh;

    [SerializeField]
    float speed = 5f;
    public float jumpSpeed = 5f;

    bool left = false;
    float dmgMult; // Damage multiplier, changes damage taken from enemies based on difficulty level. - Jason

    // Start is called before the first frame update
    void Start()
    {
        rBody = transform.GetComponent<Rigidbody>();
        mesh = transform.GetComponent<MeshCollider>();
        dmgMult = PlayerPrefs.GetFloat("difficulty"); // I am using PlayerPrefs to store difficulty setting
                                                      // between plays. - Jason
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
        }
        else if (horizontal < 0)
        {
            left = true;

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


    }

    // The player takes damage (added to interface with Enemy class.  I wanted to make it possible for
    // different enemys to give the player different amounts of damage if we decide to do so, and wanted to
    // make sure you are aware of it by adding this.  Feel free to change the name of the function, and I will
    // update the Enemy script to match, or you can suggest a different approach.)  -Jason 
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

    }

    public bool isGrounded()
    {
        RaycastHit hit1;
        Ray ray1 = new Ray(this.transform.position, (this.transform.up * -1));
        Vector3 position2 = new Vector3(this.transform.position.x - .4f, this.transform.position.y, this.transform.position.z);
        Vector3 position3 = new Vector3(this.transform.position.x + .4f, this.transform.position.y, this.transform.position.z);
        RaycastHit hit2;
        Ray ray2 = new Ray(this.transform.position, (this.transform.up * -1));
        RaycastHit hit3;
        Ray ray3 = new Ray(this.transform.position, (this.transform.up * -1));
        Physics.Raycast(ray2, out hit2);
        Physics.Raycast(ray3, out hit3);
        Physics.Raycast(ray1, out hit1);
        Vector3 hitPosition = hit1.point;
        float hitDistance1 = hit1.distance;
        float hitDistance2 = hit2.distance;
        float hitDistance3 = hit3.distance;
        Debug.Log(hitDistance1);
        return (hitDistance1 == 0 || hitDistance2 == 0 || hitDistance3 == 0);
    }
}
