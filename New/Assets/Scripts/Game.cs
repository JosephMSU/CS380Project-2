using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField]
    private GameObject _pausePrefab;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("playing", 1);
    }

    // Update is called once per frame
    void Update()
    {
        // if the esc key is pressed, open the pause menu and stop the Update functions
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            Instantiate(_pausePrefab);
            return;
        }
    }

    public void PauseMenuDestroyed()
    {
        // start the update functions again
        Time.timeScale = 1;
    }
}
