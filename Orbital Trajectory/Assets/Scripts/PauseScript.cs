using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    [SerializeField]
    GameObject pauseMenu, pauseButton, resumeButton, levelSelectButton, restartButton, mainMenuButton;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pause()
    {
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0;
    }
    public void resume()
    {
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
    }
    public void restart()
    {
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
    }
    public void levelSelect()
    {
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
    }
    public void mainMenu()
    {
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
    }

}
