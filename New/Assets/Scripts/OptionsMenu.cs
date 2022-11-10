/*
 * OptionsMenu.cs
 * 
 * This script manages the options menu, and stores the selected volume and difficulty in the
 * PlayerPrefs. It also returns to the correct menu after the player leaves the options menu.
 *     
 * This script is attached to the options menu
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    private Slider _bar;
    [SerializeField]
    private Text _volText;
    [SerializeField]
    private Image _selected;
    [SerializeField]
    private Button[] _difficultyButtons;
    [SerializeField]
    private GameObject _pauseMenuPrefab;
    [SerializeField]
    private GameObject _LooseMenuPrefab;
    [SerializeField]
    private GameObject _WinMenuPrefab;
    [SerializeField]
    private AudioMixer _mixer;
    [SerializeField]
    private AudioSource _buttonClickSound;

    void Start()
    {
        // deactivate the difficulty buttons if the game is playing.
        if (PlayerPrefs.GetInt("playing") == 1 && GameOverMenu.gameOver == false)
        {
            _selected.color = new Color(0.25f, 0.25f, 0.25f);
            foreach (Button b in _difficultyButtons)
            {
                b.interactable = false;
            }
        }

        // set the previously selected difficulty as selected
        float difficulty = PlayerPrefs.GetFloat("difficulty");

        Vector3 pos = _selected.transform.localPosition;

        if (difficulty == 0.5f)
            pos.x = -150;
        else if (difficulty == 1)
            pos.x = 0;
        else
            pos.x = 150;

        _selected.transform.localPosition = pos;

        // set the volume slider's value
        _bar.value = PlayerPrefs.GetFloat("volume");

    }

    public void VolumeChanged()
    {
        // change the text below the slider
        string volume = Mathf.RoundToInt((_bar.value * 100)).ToString();
        _volText.text = volume + "%";

        // set the volume
        PlayerPrefs.SetFloat("volume", _bar.value);
        _mixer.SetFloat("soundVolume", Mathf.Log10(_bar.value) * 20);
    }

    public void ExitButtonPushed()
    {
        // Hide the options menu so it can still play the sound, but isn't seen.
        this.gameObject.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        Vector3 pos = transform.position;
        pos.y -= 999999;
        transform.position = pos;

        // play sound
        _buttonClickSound.Play(0);
        _buttonClickSound.time = 0.2f;

        // show win or loose menu if game is over
        if (GameOverMenu.gameOver)
        {
            if (GameOverMenu.win)
                Instantiate(_WinMenuPrefab);
            else
                Instantiate(_LooseMenuPrefab);
        }
        // show the pause menu if the game is playing
        else if (PlayerPrefs.GetInt("playing") == 1)
            Instantiate(_pauseMenuPrefab);

        // exit the options menu
        StartCoroutine("Exit");
    }

    IEnumerator Exit()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(this.gameObject);
    }

    public void DifficultyButtonPushed(float difficulty)
    {
        _buttonClickSound.Play(0);
        _buttonClickSound.time = 0.2f;

        // set the difficulty
        PlayerPrefs.SetFloat("difficulty", difficulty);

        // set the selected difficulty as selected
        Vector3 pos = _selected.transform.localPosition;

        if (difficulty == 0.5f)
            pos.x = -150;
        else if (difficulty == 1)
            pos.x = 0;
        else
            pos.x = 150;

        _selected.transform.localPosition = pos;
    }
}