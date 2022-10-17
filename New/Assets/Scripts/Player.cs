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
//=======
//dmgMult = PlayerPrefs.GetFloat("difficulty"); // I am using PlayerPrefs to store difficulty setting
                                                      // between plays. - Jason
//>>>>>>> 5deed22a04c39d5315dfa3edc5047e931df575c8*/
    }

    // Update is called once per frame
    void Update()
    {

        /*Vector3 down = transform.TransformDirection(Vector3.down) * 10;
        Debug.DrawRay(transform.position, down, Color.green);*/
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
            if (isGrounded())
            {
                rBody.velocity = Vector3.up * jumpSpeed;
            }
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
        
    }

    public bool isGrounded()
    {
        RaycastHit hit;
        Ray ray = new Ray(this.transform.position, (this.transform.up * -1));
        Physics.Raycast(ray, out hit);
        Vector3 hitPosition = hit.point;
        float hitDistance = hit.distance;
        Debug.Log(hitDistance);
        return (hitDistance == 0);
    }
}
