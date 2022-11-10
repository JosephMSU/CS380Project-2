/*
 * Player.cs
 * 
 * This script manages the player, and controls player movement through user input.  It
 * also intantiates a win screen if the player reaches the goal, or a loose screen if 
 * the player runs out of health, is crushed, or falls in an infinie pit. It also allows
 * the player to shoot projectiles if the gun isn't in the process of reloading 
 * (also controlled by this script).
 *     
 * This script is attached to the player character.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Added for interfacing with the enemy script. Feel free to change the name or
    // suggest a different method.  - Jason (remove comment before release)

    [HideInInspector]
    public bool doNotMove = false;
    [HideInInspector]
    public static bool invincible;
    [HideInInspector]
    public static bool infinitePit = false;

    public Renderer playerRenderer;

    [SerializeField]
    private AudioSource walkSound;
    [SerializeField]
    private AudioSource hurtSound;
    [SerializeField]
    private AudioSource gunSound;
    [SerializeField]
    private AudioSource hitGroundSound;
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private GameObject _looseScreen;
    [SerializeField]
    private GameObject _winScreen;
    [SerializeField]
    private float _minimumY = -10f;
    [SerializeField]
    private float _invincibleTime = 2;
    [SerializeField]
    private float jumpSpeed = 10f;
    [SerializeField]
    private int health = 10;
    [SerializeField]
    private float reloadTime = 1.5f;

    [SerializeField]
    private GameObject Projectile;
    [SerializeField]
    private Text _healthLeft;
    [SerializeField]
    private GameObject _reloadTxt;

    private bool left = false;
    private bool reloading = false;
    private bool hitWall = false;
    private float dmgMult;
    private float invincibilityTime = 0;
    private float flashLength = .2f;
    private Rigidbody rBody;
    private MeshCollider mesh;
    private Animator myAnim;
    private bool _isWalking = false;
    private bool _inAir = false;
    private bool _inAirLongEnough = false;

    public int GetHealth()
    {
        return health;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetFloat("difficulty") == 0.5f)
            health *= 2;
        else if (PlayerPrefs.GetFloat("difficulty") == 2f)
            health /= 2;

        _healthLeft.text = "Health:  <b>" + health + "</b>";

        myAnim = GetComponent<Animator>();
        rBody = transform.GetComponent<Rigidbody>();
        mesh = transform.GetComponent<MeshCollider>();
        dmgMult = PlayerPrefs.GetFloat("difficulty"); // I am using PlayerPrefs to store difficulty setting
                                                      // between plays. - Jason (remove comment before release)
        //playerRenderer = transform.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0 || doNotMove)
        {
            walkSound.Pause();
            return;
        }

        if (transform.position.y < _minimumY)
        {
            Instantiate(_looseScreen);
            infinitePit = true;
        }
        /*float x = this.transform.position.x - .4f;
        float x2 = this.transform.position.x + .4f;
        Vector3 position4 = new Vector3(x, this.transform.position.y, this.transform.position.z);
        Vector3 position5 = new Vector3(x2, this.transform.position.y, this.transform.position.z);
        Vector3 down = transform.TransformDirection(Vector3.down) * 10;
        Debug.DrawRay(transform.position, down, Color.green);
        Debug.DrawRay(position4, down, Color.red);
        Debug.DrawRay(position5, down, Color.blue);*/
        float y1 = transform.position.y;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (horizontal > 0.01f)
        {
            _isWalking = true;
            left = false;
            transform.rotation = Quaternion.Euler(0, 90, 0);
            myAnim.SetInteger("X", 1);
        }
        else if (horizontal < -0.01)
        {
            _isWalking = true;
            left = true;
            transform.rotation = Quaternion.Euler(0, -90, 0);
            myAnim.SetInteger("X", 1);
        }
        else
        {
            myAnim.SetInteger("X", 0);
            _isWalking = false;
        }

        bool grounded = IsGrounded();

        if (_isWalking && grounded)
        {
            if (!walkSound.isPlaying)
            {
                walkSound.Play(0);
            }
        }
        else
        {
            walkSound.Pause();
        }

        if (grounded)
        {
            if (_inAir && _inAirLongEnough)
            {
                hitGroundSound.Play(0);
                _inAir = false;
            }
            _inAirLongEnough = false;
        }
        else
        {
            _inAir = true;
            StartCoroutine("inAirDelay");
        }

        if (!reloading && Input.GetKeyDown(KeyCode.Space))
        {
            reloading = true;
            StartCoroutine("Reload");
            shoot();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            jump();
        }
        HitWall();
        if(!hitWall)
        {
            Vector3 pos = transform.position;
            pos.x += (horizontal * Time.deltaTime * speed);
            transform.position = pos;
        } 
        if(invincible)
        {
            invincibilityTime += Time.deltaTime;
            if(invincibilityTime>=flashLength)
            {
                playerRenderer.enabled = !playerRenderer.enabled;
                invincibilityTime = 0;
            }
        }
        else if(!playerRenderer.enabled)
        {
            playerRenderer.enabled = true;
        }
    }

    public void TakeDamage(int dmgAmt)
    {
        StartCoroutine("TemporaryInvincibility");

        health -= dmgAmt;

        if (health <= 0)
        {
            health = 0;
            Instantiate(_looseScreen);
        }
        else
        {
            hurtSound.Play(0);
        }

        _healthLeft.text = "Health:  <b>" + health + "</b>";
    }

    IEnumerator TemporaryInvincibility()
    {
        invincible = true;
        yield return new WaitForSeconds(_invincibleTime);
        invincible = false;
    }

    public void jump()
    {
        if (IsGrounded())
        {
            rBody.velocity = Vector3.up * jumpSpeed;
        }
    }

    public void shoot()
    {
        // set the position of the proectile
        gunSound.Play(0);
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "winArea")
        {
            Instantiate(_winScreen);
        }
    }

    public void HitWall()
    {
        Vector3 position1 = new Vector3(this.transform.position.x, this.transform.position.y + .5f, this.transform.position.z);

        RaycastHit hitSideF;
        Ray raySideF = new Ray(position1, transform.forward);

        if (Physics.Raycast(raySideF, out hitSideF))
        {
            if (hitSideF.transform.gameObject.tag == "ground" && hitSideF.transform.rotation.z == 0)
            {
                if (hitSideF.distance <= 1)
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
    }

    public bool IsGrounded()
    {
        Vector3 position1 = new Vector3(this.transform.position.x, this.transform.position.y + .5f, this.transform.position.z);
        bool grounded = false;

        //RaycastHit hit1;
        
        Ray ray1 = new Ray(position1, (this.transform.up * -1));
        Vector3 position2 = new Vector3(this.transform.position.x - .4f, this.transform.position.y+.5f, this.transform.position.z);
        Vector3 position3 = new Vector3(this.transform.position.x + .4f, this.transform.position.y+.5f, this.transform.position.z);
        //RaycastHit hit2;
        Ray ray2 = new Ray(this.transform.position, (this.transform.up * -1));
        //RaycastHit hit3;
        Ray ray3 = new Ray(this.transform.position, (this.transform.up * -1));
        Ray ray4 = new Ray(position1, transform.up);
        RaycastHit hit;
        if(Physics.Raycast(ray4,out hit))
        {
            if(hit.transform.tag == "ground")
            {

            }
        }
        /*Physics.Raycast(ray2, out hit2);
        Physics.Raycast(ray3, out hit3);
        Physics.Raycast(ray1, out hit1);
        Debug.DrawRay(position2, this.transform.up * -1, Color.green);*/
        if(Physics.Raycast(ray1, out hit))
        {
            if(hit.transform.tag =="ground")
            {
                /*Debug.Log("Hit the ground");
                Debug.Log(hit.distance);*/
                if (hit.distance <= .7)
                {
                    grounded = true;
                    //Debug.Log(hit.distance);
                }
            }
        }
        else if(Physics.Raycast(ray2, out hit))
        {
            if(hit.transform.gameObject.tag == "ground")
            {
                if(hit.distance <= .7)
                {
                    grounded = true;
                }
            }
        }
        else if(Physics.Raycast(ray3, out hit))
        {
            if(hit.transform.gameObject.tag == "ground")
            {
                if(hit.distance <= .7)
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

    IEnumerator Reload()
    {
        // wait 2 seconds before player can shoot again
        _reloadTxt.SetActive(true);
        myAnim.SetBool("Shoot", true);

        yield return new WaitForSeconds(reloadTime);

        myAnim.SetBool("Shoot", false);
        reloading = false;
        _reloadTxt.SetActive(false);
    }

    IEnumerator inAirDelay()
    {
        yield return new WaitForSeconds(0.5f);
        _inAirLongEnough = true;
    }
}