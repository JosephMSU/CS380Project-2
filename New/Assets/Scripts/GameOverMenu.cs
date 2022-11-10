/*
 * GameOverMenu.cs
 * 
 * Manages the win and loose menus (collectively, the game over menus), which share
 * alot of similar functions. It allows the player to go to the next level (if it is
 * a win screen), retry the current levle (if it is a loose screen), access the
 * options menu, go to the main menu, or quit the game. It also tells the player their
 * final score (for a win screen), or how they lost (for a loose screen). 
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
    private GameObject _QuitScreenPrefab;

    [SerializeField]
    private Text _info;
    [SerializeField]
    private Button _nextLevel;  // null if loose menu
    [SerializeField]
    private AudioSource _gameOverSound;
    [SerializeField]
    private AudioSource _buttonClickSound;

    private static string _infoText;

    public static bool gameOver = false;
    public static bool win = false;

    public void Start()
    {
        if (!gameOver) // if this is the first time the game over screen has apperared
            _gameOverSound.Play(0);

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
            if (GameObject.FindWithTag("Player").GetComponent<Player>().GetHealth() <= 0)
                _info.text += "health";
            else if (Player.infinitePit)
                _info.text = "Fell out of\nbounds.";
            else if (GameObject.FindWithTag("MainCamera").GetComponent<Game>().timer <= 0)
                _info.text += "time";
            else
                _info.text = "ERROR";
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
        _buttonClickSound.Play(0);
        _buttonClickSound.time = 0.2f;
        Instantiate(_QuitScreenPrefab);
        StartCoroutine("Quit");
    }

    IEnumerator Quit()
    {
        yield return new WaitForSeconds(0.7f);
        QuitGame();
        Debug.Log("quit");
    }

    public void QuitGame()
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
        // Hide the options menu so it can still play the sound, but isn't seen.
        this.gameObject.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        Vector3 pos = transform.position;
        pos.y -= 999999;
        transform.position = pos;

        _buttonClickSound.Play(0);
        _buttonClickSound.time = 0.2f;

        // show the options menu
        Instantiate(_optionsPrefab);

        // exit the pause menu
        StartCoroutine("Exit");
    }

    IEnumerator Exit()
    {
        yield return new WaitForSeconds(.5f);
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