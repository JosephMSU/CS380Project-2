using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _buttonsPrefab;
    [SerializeField]
    private Button _button2;
    [SerializeField]
    private Button _button3;
    [SerializeField]
    private Text _highScore;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("highScore"))
            PlayerPrefs.SetInt("highScore", 0);

        if (!PlayerPrefs.HasKey("volume"))
            PlayerPrefs.SetFloat("volume", 1);

        if (!PlayerPrefs.HasKey("difficulty"))
            PlayerPrefs.SetFloat("difficulty", 1);

        if (!PlayerPrefs.HasKey("level"))
            PlayerPrefs.SetInt("level", 1);

        PlayerPrefs.SetInt("playing", 0);

        _highScore.text = "High Score: " + PlayerPrefs.GetInt("highScore");

        int level = PlayerPrefs.GetInt("level");
        if (level < 3)
        {
            _button3.interactable = false;
            if (level < 2)
                _button2.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OptionsButtonHit()
    {
        Instantiate(_buttonsPrefab);
    }

    public void LevelButtonHit(int level)
    {
        // \/ \/ TEMPORARY \/ \/
        if (level != 1) return;
        // \/ /\ TEMPORARY /\ /\

        SceneManager.LoadScene(level);
    }
}
