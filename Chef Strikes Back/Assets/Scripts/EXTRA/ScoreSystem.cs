using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textStats;
    [SerializeField] private Transform _gridParent;
    [SerializeField] private GameObject _star;
    [SerializeField] private GameObject _halfStar;
    [SerializeField] private GameObject _emptyStar;
    [SerializeField] private List<int> _extraMoneyForStars;

    private void Awake()
    {
        var score = ServiceLocator.Get<GameManager>().GetScore();
        _textStats.text = "Score: " + score.ToString() + "\nKill Count: " + ServiceLocator.Get<GameManager>().GetKillCount();
        ServiceLocator.Get<GameManager>().ResetScore();

        int starNum = 0;
        while (score > 9 && starNum < 5)
        {
            Instantiate(_star, _gridParent);
            score -= 10;
            ++starNum;
        }

        if (score - 5 >= 0 && starNum < 5)
        {
            Instantiate(_halfStar, _gridParent);
            ++starNum;
        }

        for (int i = starNum; i < 5; ++i)
        {
            Instantiate(_emptyStar, _gridParent);
        }

        int moneyValue = 0;
        foreach (var money in _extraMoneyForStars)
        {
            moneyValue += money;
        }
        
        ServiceLocator.Get<GameManager>().SaveMoney(moneyValue);
    }
}
