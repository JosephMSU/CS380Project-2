using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _optionsPrefab;

    public void MainMenuPushed()
    {
        // go to main menu
        SceneManager.LoadScene(0);
    }

    public void ExitButtonPushed()
    {
        // start the Update functions again
        Camera.main.GetComponent<Game>().PauseMenuDestroyed();

        // exit the pause menu
        Destroy(this.gameObject);
    }

    public void QuitButtonPushed()
    {
        Application.Quit();
    }

    public void OptionsButtonPushed()
    {
        // show the options menu
        Instantiate(_optionsPrefab);

        // exit the pause menu
        Destroy(this.gameObject);
    }
}
