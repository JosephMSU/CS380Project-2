using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Added for interfacing with the enemy script. Feel free to change the name or
    // suggest a different method.  - Jason
    [HideInInspector]
    public static bool invincible;

    [SerializeField]
    float speed = 5f;

    bool left = false;
    float dmgMult; // Damage multiplier, changes damage taken from enemies based on difficulty level. - Jason

    // Start is called before the first frame update
    void Start()
    {
        dmgMult = PlayerPrefs.GetFloat("difficulty"); // I am using PlayerPrefs to store difficulty setting
                                                      // between plays. - Jason
    }

    // Update is called once per frame
    void Update()
    {
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

        Vector3 pos = transform.position;
        pos.x += (horizontal * Time.deltaTime * speed);


    }

    // The player takes damage (added to interface with Enemy class.  I wanted to make it possible for
    // different enemys to give the player different amounts of damage if we decide to do so, and wanted to
    // make sure you are aware of it by adding this.  Feel free to change the name of the function, and I will
    // update the Enemy script to match, or you can suggest a different approach.)  -Jason 
    public void TakeDamage(int dmgAmt)
    {
        int totalDamage = (int)(dmgAmt * dmgMult);
        if (totalDamage == 0)
            totalDamage = 1;


    }
}
