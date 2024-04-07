using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private StatefulObject _screens;
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private Toggle _usingControllerButton;

    [Header("First Seclected Options")]
    [SerializeField] private GameObject _MainMenuSeclectFirst;
    [SerializeField] private GameObject _LevelSelectFirst;
    [SerializeField] private GameObject _SettingSelectFirst;
    [SerializeField] private GameObject _CreditSelectFirst;
    [SerializeField] private GameObject _PauseSelectFirst;
    [SerializeField] private GameObject _HowToPlaySelectFirst;

    private void Start()
    {
        if (_moneyText != null)
        {
            _moneyText.text = "Money = " + ServiceLocator.Get<GameManager>().GetMoney().ToString();
        }
    }

    private void OnEnable()
    {
        var gm = ServiceLocator.Get<GameManager>();
        if (gm)
        {
            //_usingControllerButton.isOn = gm.GetControllerOption();
        }
    }

    public void SetScreenActive(string screenName)
    {
        _screens.SetState(screenName);
        switch (screenName)
        {
            case "Root - Level Select":
                {
                    if (_LevelSelectFirst == null) return;
                    EventSystem.current.SetSelectedGameObject(_LevelSelectFirst);
                    break;
                }
            case "Root - Settings":
                {
                    if (_SettingSelectFirst == null) return;
                    EventSystem.current.SetSelectedGameObject(_SettingSelectFirst);
                    break;
                }
            case "Root - Main Menu":
                {
                    if (_MainMenuSeclectFirst == null) return;
                    EventSystem.current.SetSelectedGameObject(_MainMenuSeclectFirst);
                    break;
                }
            case "Root - Credits":
                {
                    if (_MainMenuSeclectFirst == null) return;
                    EventSystem.current.SetSelectedGameObject(_CreditSelectFirst);
                    break;
                }
            case "Root - Pause Menu":
                {
                    if (_PauseSelectFirst == null) return;
                    EventSystem.current.SetSelectedGameObject(_PauseSelectFirst);
                    break;
                }
            case "Root - How To Play":
                {
                    if (_HowToPlaySelectFirst == null) return;
                    EventSystem.current.SetSelectedGameObject(_HowToPlaySelectFirst);
                    break;
                }
            default:
                return;
        }
    }

    public void ResumeGame()
    {
        var inputs = ServiceLocator.Get<Player>().GetComponent<PlayerInputs>();
        inputs.TogglePauseMenu();
    }

    public void ActiveateController()
    {
        ServiceLocator.Get<GameManager>().ToggleController();
    }
}
