using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField]
    private GameObject _pausePrefab;

    private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("playing", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (paused)
            return;

        if ((Input.GetKeyDown(KeyCode.Escape)))
        {
            Instantiate(_pausePrefab);
            paused = true;
            return;
        }
    }

    public void PauseMenuDestroyed()
    {
        paused = false;
    }
}
