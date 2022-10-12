using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float speed = 5.0001f;
    bool left = false;
    // Start is called before the first frame update
    void Start()
    {
        
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
}
