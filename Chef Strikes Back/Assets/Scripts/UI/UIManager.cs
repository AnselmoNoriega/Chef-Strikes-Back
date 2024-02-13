using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private StatefulObject _screens;
    [SerializeField] private TextMeshProUGUI _moneyText;

    private void Start()
    {
        if (_moneyText != null)
        {
            _moneyText.text = "Money = " + ServiceLocator.Get<GameManager>().GetMoney().ToString();
        }
    }

    public void SetScreenActive(string screenName)
    {
        _screens.SetState(screenName);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
