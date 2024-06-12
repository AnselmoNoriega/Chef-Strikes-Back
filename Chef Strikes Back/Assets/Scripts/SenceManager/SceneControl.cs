using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneControl : MonoBehaviour
{
    private GameManager _gameManager;
    private GlobalInput _globalControl;
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private GameObject _purchasePanel;
    [SerializeField] private GameObject _noMoneyText;
    private int _currentLevelSelected;

    [Space, Header("Level UI")]
    [SerializeField] private GameObject[] _firstSelectedButton;
    private bool _isButtonInUI = true;
    private int _uiIndex = -1;
    //private AudioManager _audioManager;


    private void Start()
    {
        _gameManager = ServiceLocator.Get<GameManager>();
        _globalControl = ServiceLocator.Get<GlobalInput>();
        //_audioManager = ServiceLocator.Get<AudioManager>();
        SetButtonSelected(0);
        if (_buttons.Count > 0)
        {
            _gameManager.SetLockedLevels(_buttons);
        }
    }

    private void Update()
    {
        if (_globalControl.IsUsingController() && !_isButtonInUI && _uiIndex >= 0)
        {
            StartCoroutine(_gameManager.UI_Navegation.SetSelected(_firstSelectedButton[_uiIndex]));
            _isButtonInUI = true;
        }
        else if (!_globalControl.IsUsingController() && _isButtonInUI)
        {
            StartCoroutine(_gameManager.UI_Navegation.SetSelected(null));
            _isButtonInUI = false;
        }
    }

    public void GoToEndScene()
    {
        //_audioManager.PlaySource("ButtonPress");
        Debug.Log("ButtonPress");
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
        if (_firstSelectedButton != null && _globalControl.IsUsingController())
        {
            if (_firstSelectedButton.Length > 0)
            {
                //_audioManager.PlaySource("ButtonPress");
                Debug.Log("ButtonPress");
                StartCoroutine(_gameManager.UI_Navegation.SetSelected(_firstSelectedButton[uiLayer]));
            }
        }
        else
        {
            _isButtonInUI = false;
        }

        _uiIndex = uiLayer;
    }

    public void Go2Level(int level)
    {

        if (_gameManager.IsLevelLocked(level))
        {

            _currentLevelSelected = level;
            _purchasePanel.SetActive(true);
            SetButtonSelected(4);
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
