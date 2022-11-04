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
    private Text _info;
    [SerializeField]
    private Button _nextLevel;  // null if loose menu

    private static string _infoText;

    public static bool gameOver = false;
    public static bool win = false;

    public void Start()
    {
        Time.timeScale = 0;
        gameOver = true;

        if (_nextLevel != null) // win screen
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (sceneIndex == 3)
                _nextLevel.interactable = false;
            else if (PlayerPrefs.GetInt("level") == sceneIndex)
                PlayerPrefs.SetInt("level", sceneIndex + 1);

            if (win) // if the win screen has already appeared before
            {
                _info.text = _infoText;
                return;
            }

            // get the multiplier values
            int difficultyMultiplier;
            int timeMultiplier;
            int healthMultiplier = GameObject.FindWithTag("Player").GetComponent<Player>().GetHealth();

            float difficulty = PlayerPrefs.GetFloat("difficulty");
            if (difficulty == 0.5) // easy
            {
                difficultyMultiplier = 1;
            }
            else if (difficulty == 1) // medium
            {
                difficultyMultiplier = 2;
            }
            else // hard
            {
                difficultyMultiplier = 3;
            }

            Game g = GameObject.FindWithTag("MainCamera").GetComponent<Game>();

            // Apply multipliers to the score
            timeMultiplier = g.GetTimeLeft();
            int initialScore = g.score;
            if (initialScore == 0)
                g.score = 1;
            g.score *= healthMultiplier;
            g.score *= timeMultiplier;
            g.score *= difficultyMultiplier;
            g.UpdateScore(0);
            g.UpdateHighScore();

            // Show the score and multipliers

            _infoText = "Initial score: <b>" + initialScore + "</b>\n\n"
                + "Difficulty Multiplier: <b>" + difficultyMultiplier + "</b>\n\n"
                + "Health Multiplier: <b>" + healthMultiplier + "</b>\n\n"
                + "Time Multiplier: <b>" + timeMultiplier + "</b>\n\n\n"
                + "Final Score: <b>" + g.score + "</b>";

            _info.text = _infoText;

            win = true;
        }
        else // loose screen
        {
            if (Player.infinitePit)
                _info.text = "Fell out of\nbounds.";
            else if (GameObject.FindWithTag("MainCamera").GetComponent<Game>().timer <= 0)
                _info.text += "time";
            else
                _info.text += "health";
        }
    }

    public void MainMenuPushed()
    {
        Time.timeScale = 1;
        gameOver = false;
        win = false;
        Player.infinitePit = false;

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
        win = false;
        Player.infinitePit = false;

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
        Player.infinitePit = false;

        // go to the next level
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex + 1);
    }
}