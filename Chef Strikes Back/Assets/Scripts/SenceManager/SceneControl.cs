using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneControl : MonoBehaviour
{
    private GameManager _gameManager;
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private GameObject _purchasePanel;
    [SerializeField] private GameObject _noMoneyText;
    private int _currentLevelSelected;

    private void Start()
    {
        _gameManager.SetLockedLevels(_buttons);
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
            _currentLevelSelected = level;
            _purchasePanel.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("Level_" + level.ToString());
        }
    }

    public void UnlockLevel()
    {
        if (_gameManager.UnlockLevel(_currentLevelSelected))
        {
            _purchasePanel.SetActive(false);
            var colors = _buttons[_currentLevelSelected].colors;
            colors.normalColor = Color.white;
            _buttons[_currentLevelSelected].colors = colors;
        }
        else
        {
            _noMoneyText.SetActive(true);
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
