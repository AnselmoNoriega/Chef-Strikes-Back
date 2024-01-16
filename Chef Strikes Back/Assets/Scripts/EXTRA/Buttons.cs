using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons: MonoBehaviour
{
    public static bool GameIsPaused = false;
    private void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
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

    public void TogglePause()
    {
        if (GameIsPaused)
            Resume();
        else
            Pause();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
