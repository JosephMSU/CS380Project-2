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
    private AudioMixer _mixer;

    void Start()
    {
        if (PlayerPrefs.GetInt("playing") == 1)
        {
            _selected.color = new Color(0.25f, 0.25f, 0.25f);
            foreach (Button b in _difficultyButtons)
            {
                b.interactable = false;
            }
        }

        float difficulty = PlayerPrefs.GetFloat("difficulty");

        Vector3 pos = _selected.transform.localPosition;

        if (difficulty == 0.5f)
            pos.x = -200;
        else if (difficulty == 1)
            pos.x = 0;
        else
            pos.x = 200;

        _selected.transform.localPosition = pos;

        _bar.value = PlayerPrefs.GetFloat("volume");

    }

    public void VolumeChanged()
    {
        string volume = Mathf.RoundToInt((_bar.value * 100)).ToString();
        _volText.text = volume + "%";

        PlayerPrefs.SetFloat("volume", _bar.value);
        _mixer.SetFloat("soundVolume", Mathf.Log10(_bar.value) * 20);
    }

    public void ExitButtonPushed()
    {
        if (PlayerPrefs.GetInt("playing") == 1)
            Instantiate(_pauseMenuPrefab);

        Destroy(this.gameObject);
    }

    public void DifficultyButtonPushed(float difficulty)
    {
        PlayerPrefs.SetFloat("difficulty", difficulty);

        Vector3 pos = _selected.transform.localPosition;

        if (difficulty == 0.5f)
            pos.x = -200;
        else if (difficulty == 1)
            pos.x = 0;
        else
            pos.x = 200;

        _selected.transform.localPosition = pos;
    }
}
