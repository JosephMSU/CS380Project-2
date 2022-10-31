using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField]
    private GameObject _pausePrefab;
    [HideInInspector]
    public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("playing", 1);
    }

    /* 
    void OnApplicationFocus(bool focus)
    {
        if (!focus && !paused)
        {
            paused = true;
            Time.timeScale = 0;
            Instantiate(_pausePrefab);
        }
    }
    */

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
            return;

        // if the esc key is pressed, open the pause menu and stop the Update functions
        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            paused = true;
            Time.timeScale = 0;
            Instantiate(_pausePrefab);
            return;
        }
    }
}
