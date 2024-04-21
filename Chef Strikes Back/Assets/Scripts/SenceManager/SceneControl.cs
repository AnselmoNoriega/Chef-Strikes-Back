using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControl : MonoBehaviour
{
    private GameManager _gameManager;
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private GameObject _purchasePanel;
    [SerializeField] private GameObject _noMoneyText;
    private int _currentLevelSelected;

    [Space, Header("Level UI")]
    [SerializeField] private GameObject[] _firstSelectedButtons;

    private void Start()
    {
        _gameManager = ServiceLocator.Get<GameManager>();
        _gameManager.UI_Navegation.UIEnter(_firstSelectedButtons);
        StartCoroutine(SetFirstButton());
        if (_buttons.Count > 0)
        {
            _gameManager.SetLockedLevels(_buttons);
        }
    }

    private IEnumerator SetFirstButton()
    {
        yield return new WaitForSeconds(0.2f);
        _gameManager.UI_Navegation.SelectButton(0);
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

    public void SetButtonSelected(int uiLayer)
    {
        _gameManager.UI_Navegation.SelectButton(uiLayer);
    }

    public void Go2Level(int level)
    {
        if (_gameManager.IsLevelLocked(level))
        {
            _currentLevelSelected = level;
            _purchasePanel.SetActive(true);
            SetButtonSelected(4);

            SetButtonsInteractable(false);
        }
        else
        {
            Time.timeScale = 1.0f;
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
            SetButtonSelected(3);

            SetButtonsInteractable(true);
        }
        else
        {
            _noMoneyText.SetActive(true);
        }
    }

    public void SetButtonsInteractable(bool interactable)
    {
        foreach (var button in _buttons)
        {
            button.interactable = interactable;
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
