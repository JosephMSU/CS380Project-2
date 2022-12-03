/*
 * MainMenu.cs
 *
 * Manages the main menu, initializes the PlayerPrefs (our means of storing
 * data between plays) if they don't exist yet, and sets the game's volume
 * to the value stored in the PlayerPrefs. It also moves the highscore texts
 * for each level across the top of the screen in the main menu.
 *     
 * This script is attached to the MenuCanvas object in the main menu scene.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _optionsPrefab;
    [SerializeField]
    private GameObject _quitScreenPrefab;
    [SerializeField]
    private GameObject _loadScreenPrefab;

    [SerializeField]
    private Button _button2;
    [SerializeField]
    private Button _button3;
    [SerializeField]
    private Text[] _highScores;
    [SerializeField]
    private AudioMixer _mixer;
    [SerializeField]
    private AudioSource _buttonSound;
    [SerializeField]
    private float _txtMovSpeed;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("DestroyNotDestroyedOnLoad");
        DontDestroyOnLoad(_buttonSound);

        BossFight.fightStarted = false;
        GameOverMenu.gameOver = false;
        Time.timeScale = 1;
        StartCoroutine("HighScoreText");

        // set info used between plays to default values, if they don't exist
        if (!PlayerPrefs.HasKey("highScore1"))
            PlayerPrefs.SetInt("highScore1", 0);

        if (!PlayerPrefs.HasKey("highScore2"))
            PlayerPrefs.SetInt("highScore2", 0);

        if (!PlayerPrefs.HasKey("highScore3"))
            PlayerPrefs.SetInt("highScore3", 0);

        if (!PlayerPrefs.HasKey("volume"))
            PlayerPrefs.SetFloat("volume", 1);

        if (!PlayerPrefs.HasKey("difficulty"))
            PlayerPrefs.SetFloat("difficulty", 1);

        if (!PlayerPrefs.HasKey("level"))
            PlayerPrefs.SetInt("level", 1);

        PlayerPrefs.SetInt("playing", 0);

        // set the volume
        _mixer.SetFloat("soundVolume", Mathf.Log10(PlayerPrefs.GetFloat("volume")) * 20);

        // show the high score
        for (int i = 1; i < 4; i++)
        {
            _highScores[i - 1].text = "Level " + i + " High Score: <b>" + PlayerPrefs.GetInt("highScore" + i) + "</b>";
        }

        // determine which levels have been played
        /*
        int level = PlayerPrefs.GetInt("level");
        if (level < 3)
        {
            _button3.interactable = false;
            if (level < 2)
                _button2.interactable = false;
        }*/
    }

    IEnumerator DestroyNotDestroyedOnLoad()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("dontDestroyOnLoad"))
        {
            if (go.name != "ButtonClickSound")
            {
                AudioSource buttonSound = GameObject.FindGameObjectWithTag("dontDestroyOnLoad").GetComponent<AudioSource>();

                while (buttonSound.isPlaying)
                    yield return null;

                Destroy(buttonSound.gameObject);
            }
        }
    }

    IEnumerator HighScoreText()
    {
        while (true)
        {
            foreach (Text score in _highScores)
            {
                Vector3 pos = score.transform.localPosition;
                pos.x -= Time.deltaTime * _txtMovSpeed;
                if (pos.x <= -900)
                    pos.x = 900;
                score.transform.localPosition = pos;
            }

            // wait for next frame
            yield return null;
        }
    }

    public void OptionsButtonHit()
    {
        _buttonSound.Play(0);
        _buttonSound.time = 0.2f;
        Instantiate(_optionsPrefab);
    }

    public void QuitButtonHit()
    {
        _buttonSound.Play(0);
        _buttonSound.time = 0.2f;
        Instantiate(_quitScreenPrefab);
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

    public void LevelButtonHit(int level)
    {
        _buttonSound.Play(0);
        _buttonSound.time = 0.2f;

        // load the next scene
        Instantiate(_loadScreenPrefab);
        SceneManager.LoadSceneAsync(level);
    }
}