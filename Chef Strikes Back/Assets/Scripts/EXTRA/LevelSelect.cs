using TMPro;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;

    private void Awake()
    {
        int moneyLoaded = ServiceLocator.Get<SaveSystem>().Load<int>("money.doNotOpen");
        ServiceLocator.Get<GameManager>().AddMoney(moneyLoaded);
        _moneyText.text = ServiceLocator.Get<GameManager>().GetMoneyAmt().ToString();
    }
}
