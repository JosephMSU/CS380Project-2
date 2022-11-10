/*
 * PauseMenu.cs 
 * 
 * This script manages the pause menu.
 *     
 * This script is attached to the pause menu.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _optionsPrefab;
    [SerializeField]
    private GameObject _QuitScreenPrefab;

    [SerializeField]
    private AudioSource _buttonClickSound;

    public void MainMenuPushed()
    {
        Time.timeScale = 1;

        // go to main menu
        SceneManager.LoadScene(0);
    }

    public void ExitButtonPushed()
    {
        // Hide the options menu so it can still play the sound, but isn't seen.
        this.gameObject.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        Vector3 pos = transform.position;
        pos.y -= 999999;
        transform.position = pos;

        _buttonClickSound.Play(0);
        _buttonClickSound.time = 0.2f;

        // start the update functions again
        Time.timeScale = 1;

        // unpause the game
        GameObject.FindWithTag("MainCamera").GetComponent<Game>().paused = false;
        StartCoroutine("Exit");
    }

    IEnumerator Exit()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(this.gameObject);
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
}