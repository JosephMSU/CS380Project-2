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
    private Button _button2;
    [SerializeField]
    private Button _button3;
    [SerializeField]
    private Text _highScore;
    [SerializeField]
    private AudioMixer _mixer;

    // Start is called before the first frame update
    void Start()
    {
        // set info used between plays to default values, if they don't exist
        if (!PlayerPrefs.HasKey("highScore"))
            PlayerPrefs.SetInt("highScore", 0);

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
        _highScore.text = "High Score: " + PlayerPrefs.GetInt("highScore");

        // determine which levels have been played
        int level = PlayerPrefs.GetInt("level");
        if (level < 3)
        {
            _button3.interactable = false;
            if (level < 2)
                _button2.interactable = false;
        }
    }

    public void OptionsButtonHit()
    {
        Instantiate(_optionsPrefab);
    }

    public void LevelButtonHit(int level)
    {
        // \/ \/ TEMPORARY \/ \/
        if (level != 1) return;
        // \/ /\ TEMPORARY /\ /\

        // load the next scene
        SceneManager.LoadScene(level);
    }
}
