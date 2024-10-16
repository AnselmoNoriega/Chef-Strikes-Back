using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class Levels
{
    public int Price;
    public bool IsLock;
    public bool AllStarsAchieved;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] bool _isInDebug = true;

    [Header("System")]
    [SerializeField] public InputsUI UI_Navegation;

    [Header("Levels")]
    [SerializeField] private List<Levels> _levelsLocked = new();

    [Header("Game Variables")]
    [SerializeField] private int _customerMadPoints;
    [SerializeField] private int _killPoints;
    [SerializeField] private int _grabMoneyPoints;

    private bool _isUsingController = false;

    [SerializeField] private int _score = 0;
    private int _levelKillCount = 0;

    private string _lastScenePlayed;
    private int _money = 0;

    public void LoadLevels()
    {
        var levelsLoaded = ServiceLocator.Get<SaveSystem>().Load<List<Levels>>("levels.doNotOpen");
        if (levelsLoaded != null)
        {
            _levelsLocked = levelsLoaded;
        }
    }

    public void LoadGameStats()
    {
        _money = ServiceLocator.Get<SaveSystem>().Load<int>("money.doNotOpen");
        if(_isInDebug)
        {
            _money = 100000;
        }
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

    public int GetKillCount()
    {
        return _levelKillCount;
    }

    public void SetKillCount(int amt)
    {
        _levelKillCount = amt;
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

    public void ToggleController()
    {
        _isUsingController = !_isUsingController;
        var characterInputs = ServiceLocator.Get<Player>();
        if (characterInputs)
        {
            characterInputs.GetComponent<PlayerInputs>().SetControllerActive(_isUsingController);
        }
    }

    public bool GetControllerOption()
    {
        return _isUsingController;
    }

    public bool UnlockLevel(int lv)
    {
        if (lv == 6)
        {
            for (int i = 0; i < 6; ++i)
            {
                if (!_levelsLocked[lv].AllStarsAchieved)
                {
                    return false;
                }
            }

            SaveLevels();
            return true;
        }
        if (_money >= _levelsLocked[lv].Price)
        {
            _levelsLocked[lv].IsLock = false;
            _money -= _levelsLocked[lv].Price;
            SaveLevels();
            return true;
        }

        return false;
    }

    public void SetLockedLevels(List<Button> buttons)
    {
        if(_isInDebug)
        {
            for (int i = 0; i < _levelsLocked.Count; ++i)
            {
                if (_levelsLocked[i].IsLock)
                {
                    _levelsLocked[i].IsLock = false;
                }
            }
            return;
        }

        for (int i = 0; i < _levelsLocked.Count; ++i)
        {
            if (_levelsLocked[i].IsLock)
            {
                var colors = buttons[i].colors;
                colors.normalColor = Color.gray;
                buttons[i].colors = colors;
            }
        }
    }

    public void FullStarsForLevel(int lv)
    {
        _levelsLocked[lv].AllStarsAchieved = true;
        SaveLevels();
    }

    public bool IsLevelLocked(int lv)
    {
        return _levelsLocked[lv].IsLock;
    }

    private void SaveLevels()
    {
        ServiceLocator.Get<SaveSystem>().Save<List<Levels>>(_levelsLocked, "levels.doNotOpen");
    }
}
