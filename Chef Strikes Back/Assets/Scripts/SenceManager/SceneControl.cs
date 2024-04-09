using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public struct Levels
{
    public int Price;
    public Button LevelButtons;
    public bool IsLock;
    public bool AllStarsAchieved;
}

public class SceneControl : MonoBehaviour
{
    [SerializeField] private List<Levels> _levelLocks;

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
        if(_levelLocks[level].IsLock)
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
        if (ServiceLocator.Get<GameManager>().UnlockLevel(_levelLocks[level - 1 ]))
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
