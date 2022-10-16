using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    private Scrollbar _bar;
    [SerializeField]
    private Text _volText;
    [SerializeField]
    private Image _selected;
    [SerializeField]
    private Button[] _difficultyButtons;
    [SerializeField]
    private GameObject _pauseMenuPrefab;

    void Start()
    {
        if (PlayerPrefs.GetInt("Playing") == 0)
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

        if (volume == "100")
            volume = "1";
        else
            volume = "0." + volume;

        PlayerPrefs.SetFloat("volume", float.Parse(volume));
    }

    public void ExitButtonPushed()
    {
        if (PlayerPrefs.GetInt("Playing") == 1)
            Instantiate(_pauseMenuPrefab);

        Destroy(this.gameObject);
    }

    public void mPushed()
    {
        
    }

    public void quitButtonPushed()
    {
        Application.Quit();
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
