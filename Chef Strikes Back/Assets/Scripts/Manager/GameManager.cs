using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _enterToRageModePoints;
    [SerializeField] private int _killPoints;
    [SerializeField] private int _grabMoneyPoints;
    [SerializeField] private int _foodMadePoints;

    private int _money;

    private int _score = 0;

    public void EnterRageModeScore()
    {
        _score += _enterToRageModePoints;
    }

    public void KillScoreUpdate()
    {
        _score += _killPoints;
    }

    public void MoneyGrabed()
    {
        _score += _grabMoneyPoints;
    }

    public void FoodGiven()
    {
        _score += _foodMadePoints;
    }

    public int GetScore()
    {
        return _score;
    }

    public void ResetScore()
    {
        _score = 0;
    }

    public void AddMoney(int amt)
    {
        _money += amt;
    }

    public void SaveMoney()
    {
        int moneyBank = ServiceLocator.Get<GameManager>().GetMoneyAmt();
        int totalMoney = moneyBank + _money;
        ServiceLocator.Get<SaveSystem>().Save<int>(totalMoney, "money.doNotOpen");
    }

    public int GetMoneyAmt()
    {
        return _money;
    }
}
