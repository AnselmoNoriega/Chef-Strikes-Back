using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct StarsWorth
{
    public string LevelName;
    public List<int> _extraMoneyForStars;
}
public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textStats;
    [SerializeField] private Transform _gridParent;
    [SerializeField] private GameObject _star;
    [SerializeField] private GameObject _halfStar;
    [SerializeField] private GameObject _emptyStar;
    [SerializeField] private List<StarsWorth> _starsWorths;

    private void Awake()
    {
        var score = ServiceLocator.Get<GameManager>().GetScore();
        _textStats.text = "Score: " + score.ToString() + "\nKill Count: " + ServiceLocator.Get<GameManager>().GetKillCount();
        ServiceLocator.Get<GameManager>().ResetScore();
        int starNum = 0;

        if(score >= 50)
        {
            var sceneName = ServiceLocator.Get<GameManager>().GetRepalyScene();
            var level = SceneManager.GetSceneByName(sceneName).buildIndex - 3;
            ServiceLocator.Get<GameManager>().FullStarsForLevel(level);
        }

        do
        {
            Instantiate(_star, _gridParent);
            score -= 10;
            ++starNum;
        }
        while (score > 9 && starNum < 5);

        if (score - 5 >= 0 && starNum < 5)
        {
            Instantiate(_halfStar, _gridParent);
            ++starNum;
        }

        for (int i = starNum; i < 5; ++i)
        {
            Instantiate(_emptyStar, _gridParent);
        }

        foreach (var startworth in _starsWorths)
        {
            if (ServiceLocator.Get<SceneControl>().GetSceneName(startworth.LevelName))
            {
                int moneyValue =0;
                for (int i = 0; i < startworth._extraMoneyForStars.Count; i++)
                {
                    if (i == starNum)
                    {
                        moneyValue = startworth._extraMoneyForStars[i];
                    }
                }
                ServiceLocator.Get<GameManager>().SaveMoney(moneyValue);
                break;
            }
        }
    }
}

