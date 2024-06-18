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

    [SerializeField] private bool _useConfetti = false;
    //private AudioManager _audioManager;
    private bool IsPaused;
    private int escapeKeyPressCount = 0;

    private void Start()
    {
        if (_moneyText != null)
        {
            _moneyText.text = "Money = " + ServiceLocator.Get<GameManager>().GetMoney().ToString();
        }

        //_audioManager = ServiceLocator.Get<AudioManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && IsPaused == true) 
        {
            //_audioManager.PlaySource("UIClick");
            Debug.Log("UIClick");
        }

    }
    public void SetScreenActive(string screenName)
    {
        _screens.SetState(screenName);

    }

    public void ResumeGame()
    {
        var inputs = ServiceLocator.Get<Player>().GetComponent<PlayerInputs>();
        inputs.TogglePauseMenu();
        IsPaused = false;
    }

    public void ActiveateController()
    {
        ServiceLocator.Get<GameManager>().ToggleController();
    }

    public void ToggleConfettiEffect()
    {
        AI.ToggleUseConfetti(!AI.UseConfetti); // Toggle and apply new state
        Debug.Log("Confetti On");
    }


}
