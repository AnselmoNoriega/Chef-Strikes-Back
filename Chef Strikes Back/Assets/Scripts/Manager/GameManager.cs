using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _customerMadPoints;
    [SerializeField] private int _killPoints;
    [SerializeField] private int _grabMoneyPoints;

    [SerializeField] private int _score = 0;
    private string _lastScenePlayed;
    private int _money = 0;

    public void LoadGameStats()
    {
        _money = ServiceLocator.Get<SaveSystem>().Load<int>("money.doNotOpen");
    }

    public void EnterRageModeScore()
    {
        _score += _customerMadPoints;
        Debug.Log("Rage " + _customerMadPoints);
    }

    public void KillScoreUpdate()
    {
        _score += _killPoints;
        Debug.Log("Kill " + _killPoints);
    }

    public void MoneyGrabed()
    {
        _score += _grabMoneyPoints;
        Debug.Log("MoneyGrabed " + _grabMoneyPoints);
    }

    public void FoodGiven(float time)
    {
        if (time >= 20.0f)
        {
            _score += 5;
            Debug.Log("FoodGiven " + 5);
        }
        else if (time >= 15.0f)
        {
            _score += 3;
            Debug.Log("FoodGiven " + 3);
        }
        else if (time >= 5.0f)
        {
            _score += 3;
            Debug.Log("FoodGiven " + 3);
        }
        else if (time >= 0.1f)
        {
            _score += 3;
            Debug.Log("FoodGiven " + 3);
        }
    }

    public int GetScore()
    {
        return _score;
    }

    public void ResetScore()
    {
        _score = 0;
    }

    public void SaveMoney(int earnings)
    {
        _money += earnings;
        ServiceLocator.Get<SaveSystem>().Save<int>(_money, "money.doNotOpen");
    }

    public int GetMoney()
    {
        return _money;
    }

    public void SetThisLevelSceneName(string name)
    {
        _lastScenePlayed = name;
    }

    public string GetRepalyScene()
    {
        return _lastScenePlayed;
    }
}
