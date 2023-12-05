using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void NextLevelPressed()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void QuittoTitlePressed()
    {
        SceneManager.LoadScene("StartScene");
    }
}
