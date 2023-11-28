using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private Transform _gridParent;
    [SerializeField] private GameObject _star;
    [SerializeField] private GameObject _halfStar;
    [SerializeField] private GameObject _emptyStar;

    private void Awake()
    {
        var score = ServiceLocator.Get<GameManager>().Score;
        Debug.Log(score);
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
        }

        for(int i = starNum; i < 5; ++i)
        {
            Instantiate(_emptyStar, _gridParent);
        }
    }
}
