/*
 * PauseMenu.cs
 * Main Author:  Jason
 * Other Authors:  
 * 
 * Manages the pause menu.
 *     
 * This script is attached to the pause menu.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _optionsPrefab;

    public void MainMenuPushed()
    {
        Time.timeScale = 1;

        // go to main menu
        SceneManager.LoadScene(0);
    }

    public void ExitButtonPushed()
    {
        // start the update functions again
        Time.timeScale = 1;

        // unpause the game
        Destroy(this.gameObject);
        GameObject.FindWithTag("MainCamera").GetComponent<Game>().paused = false;
    }

    public void QuitButtonPushed()
    {
        Application.Quit();
    }

    public void OptionsButtonPushed()
    {
        // show the options menu
        Instantiate(_optionsPrefab);

        // exit the pause menu
        Destroy(this.gameObject);
    }
}