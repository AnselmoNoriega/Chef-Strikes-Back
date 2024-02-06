using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private StatefulObject _screens;

    public void SetScreenActive(string screenName)
    {
        _screens.SetState(screenName);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
