using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControl : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public Text pausingText;
    [SerializeField]
    private InputActionReference keyPause;

    private void OnEnable()
    {
        keyPause.action.Enable();
        keyPause.action.performed += OnClicked;
    }

    private void OnDisable()
    {
        keyPause.action.Disable();
        keyPause.action.performed -= OnClicked;
    }

    private void OnClicked(InputAction.CallbackContext input)
    {
        if (SceneManager.GetActiveScene().name == "MainScene" || SceneManager.GetActiveScene().name == "MainScene2")
        {
            TogglePause();

            if (GameIsPaused) { pausingText.enabled = true; }

            if (!GameIsPaused) { pausingText.enabled = false; }
        }
    }

    public void switchToGameOverScene()
    {
        SceneManager.LoadScene("EndLevel");
    }
    public void switchToWinScene()
    {
        SceneManager.LoadScene("EndLevel");
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

    public void switchToSettingsScene()
    {
        SceneManager.LoadScene("SettingsScene");
    }

    public void switchToBookCoverScene()
    {
        SceneManager.LoadScene("BookCover");
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
        Debug.Log("paused");
        ServiceLocator.Get<AudioManager>().PlaySource("resume");
        GameIsPaused = false;
    }

    public void Pause()
    {
        UnityEngine.Debug.Log(GameIsPaused);
        Time.timeScale = 0f;
        Debug.Log("resume");
        ServiceLocator.Get<AudioManager>().PlaySource("pause");
        GameIsPaused = true;
    }


}
