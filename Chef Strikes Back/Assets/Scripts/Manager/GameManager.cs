using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _enterToRageModePoints;
    [SerializeField] private int _killPoints;
    [SerializeField] private int _grabMoneyPoints;
    [SerializeField] private int _foodMadePoints;

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

    public void FoodMade()
    {
        _score += _foodMadePoints;
    }

    public int GetScore()
    {
        return _score;
    }
}
