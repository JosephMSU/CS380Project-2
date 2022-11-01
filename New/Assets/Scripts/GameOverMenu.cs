/*
 * GameOverMenu.cs
 * Main Author:  Jason
 * Other Authors:  
 * 
 * Manages the win and loose menus (collectively, the game over menus), which share
 * alot of similar functions.
 *     
 * This script is attached to the loose and win menus.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _optionsPrefab;

    [SerializeField]
    private Text _looseReason;  // null if win menu
    [SerializeField]
    private Button _nextLevel;  // null if loose menu

    public static bool gameOver = false;
    public static bool win = false;

    public void Start()
    {
        Time.timeScale = 0;
        gameOver = true;
        if (_looseReason != null) // loose screen
        {
            if (GameObject.FindWithTag("MainCamera").GetComponent<Game>().timer <= 0)
                _looseReason.text += "time";
            else
                _looseReason.text += "health";
        }
        else if (_nextLevel != null) // win screen
        {
            win = true;
            GameObject.FindWithTag("MainCamera").GetComponent<Game>().UpdateHighScore();

            if (SceneManager.GetActiveScene().buildIndex == 3)
                _nextLevel.interactable = false;
        }

        
    }

    public void MainMenuPushed()
    {
        Time.timeScale = 1;
        gameOver = false;
        win = false;

        // go to main menu
        SceneManager.LoadScene(0);
    }

    public void QuitButtonPushed()
    {
        Application.Quit();
    }

    public void RetryButtonPushed()
    {
        Time.timeScale = 1;
        gameOver = false;
        win = false;

        // reload the level
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }

    public void OptionsButtonPushed()
    {
        // show the options menu
        Instantiate(_optionsPrefab);

        // exit the pause menu
        Destroy(this.gameObject);
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        gameOver = false;

        // go to the next level
        Scene scene = SceneManager.GetActiveScene();
        //SceneManager.LoadScene(scene.buildIndex + 1);
    }
}