using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {
    public static bool isPaused = false;
    public GameObject pauseMenu;
    public GameObject background;

    void Start() {
        pauseMenu.SetActive(false);
        background.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown (KeyCode.Space)) {
            if (isPaused) {
                Resume ();
            } else {
                Pause ();
            }
        }
    }

    void Resume () {
        pauseMenu.SetActive (false);
        background.SetActive (false);
        Time.timeScale = 1f;
        isPaused = false;

    }

    void Pause () {
        pauseMenu.SetActive (true);
        background.SetActive (true);
        Time.timeScale = 0f;
        isPaused = true;
    }
}