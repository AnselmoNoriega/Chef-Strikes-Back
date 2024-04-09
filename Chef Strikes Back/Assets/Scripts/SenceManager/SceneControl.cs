using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneControl : MonoBehaviour
{
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = ServiceLocator.Get<GameManager>();
    }

    public void GoToEndScene()
    {
        SceneManager.LoadScene("EndLevel");
    }

    public void ChangeScene(string sceneName)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(sceneName);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void Go2Level(int level)
    {
        if(_gameManager.IsLevelLocked(level))
        {
            //open panel
        }
        else
        {
            SceneManager.LoadScene("Level_" + level.ToString());
        }
    }

    public void UnlockLevel(int level)
    {
        if (ServiceLocator.Get<GameManager>().UnlockLevel(level))
        {
            //close panel
        }
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

    public void Replay()
    {
        SceneManager.LoadScene(ServiceLocator.Get<GameManager>().GetRepalyScene());
    }
}
