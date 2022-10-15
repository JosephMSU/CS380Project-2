using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("volume"))
            PlayerPrefs.SetFloat("volume", 1);

        if (!PlayerPrefs.HasKey("difficulty"))
            PlayerPrefs.SetFloat("difficulty", 1);

        PlayerPrefs.SetInt("playing", 0);
        // More stuff will probably go here.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
