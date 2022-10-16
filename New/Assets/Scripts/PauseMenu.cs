using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _optionsPrefab;

    public void MainMenuPushed()
    {

    }

    public void ExitButtonPushed()
    {
        Camera.main.GetComponent<Game>().PauseMenuDestroyed();
        Destroy(this.gameObject);
    }

    public void QuitButtonPushed()
    {
        Application.Quit();
    }

    public void OptionsButtonPushed()
    {
        Instantiate(_optionsPrefab);
        Destroy(this.gameObject);
    }
}
