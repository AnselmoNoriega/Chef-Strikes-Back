using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons: MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseText;
    public Button pauseButton;
    public Button resumeButton;
    public Button restartButton;

    private void Start()
    {
        pauseButton.onClick.AddListener(Pause);
        resumeButton.onClick.AddListener(Resume);
        restartButton.onClick.AddListener(RestartLevel);

        resumeButton.gameObject.SetActive(false); 
        restartButton.gameObject.SetActive(false);
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
        pauseText.SetActive(false);
        pauseButton.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
    }

    public void Pause()
    {
        Debug.Log("Pause method called");
        Time.timeScale = 0f;
        GameIsPaused = true;
        pauseText.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
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
