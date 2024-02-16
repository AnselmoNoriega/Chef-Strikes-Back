using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private StatefulObject _screens;
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private Toggle _usingControllerButton;

    private void Start()
    {
        if (_moneyText != null)
        {
            _moneyText.text = "Money = " + ServiceLocator.Get<GameManager>().GetMoney().ToString();
        }
    }

    private void OnEnable()
    {
        _usingControllerButton.isOn = ServiceLocator.Get<GameManager>().GetControllerOption();
    }

    public void SetScreenActive(string screenName)
    {
        _screens.SetState(screenName);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void ActiveateController()
    {
        ServiceLocator.Get<GameManager>().ToggleController();
    }
}
