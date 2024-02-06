using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControl : MonoBehaviour
{
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
