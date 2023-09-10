using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneControl : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public Text pausingText;
    private void Start()
    {

    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainScene") 
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        if (GameIsPaused) {pausingText.enabled = true;}
        if (!GameIsPaused) { pausingText.enabled=false; }
        }
    }
    public void switchToGameOverScene()
    {
        SceneManager.LoadScene("LoseScene");
    }
    public void switchToWinScene()
    {
        SceneManager.LoadScene("WinScene");
    }
    public void switchToCreditScene()
    {
        SceneManager.LoadScene("CreditScene");
    }
    public void switchToFrontScene()
    {
        SceneManager.LoadScene("FrontScene");
    }
    public void switchToGameScene()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void switchToHelpScene()
    {
        SceneManager.LoadScene("HelpScene");
    }
    public void TogglePause()
    {
        if (GameIsPaused)
            Resume();
        else
            Pause();
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        GameIsPaused = true;
    }


}
