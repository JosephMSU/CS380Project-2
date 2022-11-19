/*
 * BossFight.cs
 * 
 * Controls the boss fight. What the player reaches the boss fight area in level 3, this
 * script prepares the boss fight area and the camera for the bossfight, and makes the 
 * boss's health bar to appear.  After the boss fight, it makes the boss's health bar
 * dissapear, returns the camera's control back to the FollowCam script, and allows the
 * player to get to the goal.
 * 
 * This script is attached to the Main Camera in level 3.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossFight : MonoBehaviour
{
    [HideInInspector]
    public static bool fightStarted = false;

    [SerializeField]
    private float _bossFightStartDist;
    [SerializeField]
    private float _doorCloseSpeed;
    [SerializeField]
    private float _doorOpenSpeed;
    [SerializeField]
    private float _camZoomSpeed;
    [SerializeField]
    private float _bossHlthBarMovSpd;
    [SerializeField]
    private GameObject _closeDoor;
    [SerializeField]
    private GameObject _openDoor;
    [SerializeField]
    private GameObject _cam;
    [SerializeField]
    private GameObject _boss;
    [SerializeField]
    private GameObject _bossHlthBar;
    [SerializeField]
    private MovingPlatform[] platforms;
    [SerializeField]
    private AudioSource _doorCloseSound;

    private GameObject _hero;

    // Start is called before the first frame update
    void Start()
    {
        _hero = GameObject.FindWithTag("Player");
        StartCoroutine("WaitForFight");
        foreach (MovingPlatform plat in platforms)
        {
            plat.move = false;
        }
    }

    IEnumerator WaitForFight()
    {
        while (_hero.transform.position.x < _bossFightStartDist)
            yield return null;

        StartCoroutine("CloseDoor");
    }

    IEnumerator CloseDoor()
    {
        _cam.GetComponent<FollowCam>().follow = false;
        _hero.GetComponent<Player>().doNotMove = true;

        yield return new WaitForSeconds(0.5f);

        // Close the door
        while (_closeDoor.transform.position.y > 38.1)
        {
            yield return null;
            Vector3 dPos = _closeDoor.transform.position;
            dPos.y -= _doorCloseSpeed * Time.deltaTime;
            _closeDoor.transform.position = dPos;
        }

        _doorCloseSound.Play(0);
        StartCoroutine("ShakeCamera");
    }

    IEnumerator ShakeCamera()
    {
        Camera camera = _cam.GetComponent<Camera>();

        // Shake the camera when the door has closed
        Vector3 pos = _cam.transform.position;
        Vector3 orPos = pos;

        pos.y += 0.008f * camera.orthographicSize;
        pos.x -= 0.003f * camera.orthographicSize;
        _cam.transform.position = pos;
        yield return new WaitForSeconds(0.05f);

        pos.y -= 0.002f * camera.orthographicSize;
        pos.x += 0.007f * camera.orthographicSize;
        _cam.transform.position = pos;
        yield return new WaitForSeconds(0.05f);

        pos.y += 0.003f * camera.orthographicSize;
        pos.x -= 0.01f * camera.orthographicSize;
        _cam.transform.position = pos;
        yield return new WaitForSeconds(0.05f);

        pos.y -= 0.01f * camera.orthographicSize;
        _cam.transform.position = pos;
        yield return new WaitForSeconds(0.05f);

        pos.y -= 0.005f * camera.orthographicSize;
        pos.x += 0.003f * camera.orthographicSize;
        _cam.transform.position = pos;
        yield return new WaitForSeconds(0.05f);

        _cam.transform.position = orPos;

        yield return new WaitForSeconds(0.25f);

        if (!fightStarted)
            StartCoroutine("ZoomOut");
        else
            StartCoroutine("OpenDoor");
    }

    IEnumerator ZoomOut()
    {
        Camera camera = _cam.GetComponent<Camera>();
        Vector3 dest = new Vector3(177.8f, 28, -10);

        while (_cam.transform.position.y != dest.y ||
               _cam.transform.position.x != dest.x ||
               camera.orthographicSize < 10.5f)
        {
            if (camera.orthographicSize < 10.5f)
                camera.orthographicSize += _camZoomSpeed * 0.275f * Time.deltaTime;
            else
                camera.orthographicSize = 10.5f;

            Vector3 pos = Vector3.MoveTowards(_cam.transform.position, dest, _camZoomSpeed * Time.deltaTime);

            _cam.transform.position = pos;

            yield return null;
        }
        StartCoroutine("Fight");
    }

    IEnumerator Fight()
    {
        // Before the fight
        foreach (MovingPlatform plat in platforms)
        {
            plat.move = true;
        }
        yield return new WaitForSeconds(0.5f);
        _hero.GetComponent<Player>().doNotMove = false;
        StartCoroutine("BossHealthBarMoveIntoView");

        fightStarted = true;

        // During the fight
        Slider health = _bossHlthBar.GetComponent<Slider>();
        while (health.value != 0)
        {
            yield return null;
        }

        // After the boss is defeated
        _hero.GetComponent<Player>().doNotMove = true;

        foreach (MovingPlatform plat in platforms)
        {
            Destroy(plat.gameObject);
        }

        StartCoroutine("ShakeCamera");
        StartCoroutine("BossHealthBarMoveOutOfView");
    }

    // Reduce the area filled in health bar when boss is hit.
    public void BossHit()
    {
        _bossHlthBar.GetComponent<Slider>().value -= 1;
    }

    IEnumerator BossHealthBarMoveIntoView()
    {
        Vector3 MovTo = new Vector3(0, 225, 0);
        while (_bossHlthBar.transform.localPosition.y > MovTo.y)
        {
            Vector3 mov;
            mov = Vector3.MoveTowards(_bossHlthBar.transform.localPosition, MovTo, _bossHlthBarMovSpd * Time.deltaTime);
            _bossHlthBar.transform.localPosition = mov;
            yield return null;
        }
    }

    IEnumerator BossHealthBarMoveOutOfView()
    {
        Vector3 MovTo = new Vector3(0, 286, 0);
        while (_bossHlthBar.transform.localPosition.y < MovTo.y)
        {
            Vector3 mov;
            mov = Vector3.MoveTowards(_bossHlthBar.transform.localPosition, MovTo, _bossHlthBarMovSpd * Time.deltaTime);
            _bossHlthBar.transform.localPosition = mov;
            yield return null;
        }
    }

    IEnumerator OpenDoor()
    {

        yield return new WaitForSeconds(0.5f);

        // Open the door
        while (_openDoor.transform.position.y < 41.7f)
        {
            yield return null;
            Vector3 dPos = _openDoor.transform.position;
            dPos.y += _doorOpenSpeed * Time.deltaTime;
            _openDoor.transform.position = dPos;
        }
        Vector3 finPos = _openDoor.transform.position;
        finPos.y = 41.7f;
        _openDoor.transform.position = finPos;

        _cam.GetComponent<FollowCam>().ZoomBackToPlayer(_camZoomSpeed);
    }
}