using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneControl : MonoBehaviour
{
    //Edit by kingston  -- 8/22
    public static bool GameIsPaused = false;
    private void Start()
    {

    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Kingston") 
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }
    }
    public void switchToGameOverSence()
    {
        SceneManager.LoadScene("GameOver");
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
        SceneManager.LoadScene("Kingston");
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
