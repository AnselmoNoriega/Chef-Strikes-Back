using Newtonsoft.Json.Bson;
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
        if (keyPause)
        {
            keyPause.action.Enable();
            keyPause.action.performed += OnClicked;
        }
    }

    private void OnDisable()
    {
        if (keyPause)
        {
            keyPause.action.Disable();
            keyPause.action.performed -= OnClicked;
        }
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

    public void GoToEndScene()
    {
        SceneManager.LoadScene("EndLevel");
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void quitGame()
    {
        Application.Quit();
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

    public bool GetSceneName(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            if (name == SceneManager.GetActiveScene().name)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

}
