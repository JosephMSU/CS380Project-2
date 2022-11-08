/*
 * Game.cs
 * Main Author:  Jason
 * Other Authors: 
 * 
 * Manages the UI, and keeps track of the score and time left.  Also pauses the game then the
 * esc key is pressed, and instantiates a loose screen when the timer reaches 0.
 * 
 *     
 * This script is attached to the cameras in the level scenes (although it doesn't control the
 * cameras in any way).
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField]
    private GameObject _pausePrefab;
    [SerializeField]
    private GameObject _loosePrefab;
    [SerializeField]
    private Text _scoreTxt;
    [SerializeField]
    private Text _highScore;
    [SerializeField]
    private Text _timeLeft;

    [HideInInspector]
    public bool paused = false;
    [HideInInspector]
    public int score;

    public float timer = 120f;

    private int _levelNumber;

    public int GetTimeLeft()
    {
        return Mathf.RoundToInt(timer);
    }

    // Start is called before the first frame update
    void Start()
    {
        BossFight.fightStarted = false;
        GameOverMenu.gameOver = false;
        Player.invincible = false;
        Time.timeScale = 1;
        PlayerPrefs.SetInt("playing", 1);
        _levelNumber = SceneManager.GetActiveScene().buildIndex;
        int highScoreVal = PlayerPrefs.GetInt("highScore" + _levelNumber);
        _highScore.text = "High Score: <b>" + highScoreVal + "</b>";
        _timeLeft.text = "Time Left: <b>" + Mathf.Round(timer) + "</b>";
    }

    void OnApplicationFocus(bool focus)
    {
        if (!focus && !paused)
        {
            paused = true;
            Time.timeScale = 0;
            Instantiate(_pausePrefab);
        }
    }

    public void UpdateScore(int addScore)
    {
        score += addScore;
        _scoreTxt.text = "Score: <b>" + score + "</b>";
    }

    public void UpdateHighScore()
    {
        // if the current score is higher than the highscore, update the highscore.
        if (score > PlayerPrefs.GetInt("highScore" + _levelNumber))
        {
            PlayerPrefs.SetInt("highScore" + _levelNumber, score);
            _highScore.text = "High Score: <b>" + score + "</b>";
        }
    }

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

        // show the amount of time left
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 0;
            Instantiate(_loosePrefab);
        }
        _timeLeft.text = "Time Left: <b>" + Mathf.Round(timer) + "</b>";
    }
}