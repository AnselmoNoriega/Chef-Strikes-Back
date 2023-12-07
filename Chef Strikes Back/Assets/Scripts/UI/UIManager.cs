using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void NextLevelPressed()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void QuittoTitlePressed()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void switchToTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void switchToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void switchToSettings()
    {
        SceneManager.LoadScene("SettingsScene");
    }

    public void switchToCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void switchToGameScene()
    {
        SceneManager.LoadScene("MainScene");
    }


    public void quitGame()
    {
        Application.Quit(); 
    }
}
