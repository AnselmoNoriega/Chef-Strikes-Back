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
    [SerializeField] private TextMeshProUGUI _quoteDisplay;  // UI element to display the quote
    [SerializeField] private GameObject[] _images;  // Array of images to display

    private Dictionary<int, List<string>> quotesByStars = new Dictionary<int, List<string>>
    {
        { 1, new List<string>
            {
                "My 3 year old nephew makes better pizza.",
                "I’ve met rocks with better cooking skills.",
                "You couldn’t tell a skillet from a frying pan.",
                "You call that a diced tomato?",
                "I could do this in my sleep.",
                "I am going to shoot you.",
                "I wanna see the manager."
            }
        },
        { 2, new List<string>
            {
                "Your cooking is fine, I just don’t like your face.",
                "Nice restaurant, but I hate the cook.",
                "The food was bland and tasteless, like my husband.",
                "Pizza should not exist.",
                "Hey it’s okay, not everyone is meant to be a Chef.",
                "You better pay me back."
            }
        },
        { 3, new List<string>
            {
                "I ordered food and got it. I have nothing else to say.",
                "This was certainly a restaurant.",
                "I honestly forgot what I ordered.",
                "Fine, I’ll give a review…",
                "Is my life just a simulation?"
            }
        },
        { 4, new List<string>
            {
                "This restaurant represents everything that is wrong with our society but damn that’s a good pizza.",
                "I haven’t had such good food in a loooong time.",
                "This place has now become my go-to restaurant!",
                "I can’t wait for my next time here!",
                "Amazing pizza!",
                "Spaghet about it! This place is great!"
            }
        },
        { 5, new List<string>
            {
                "This pizza was so good it made me reconsider the core of my beliefs.",
                "I understand now…",
                "I think I’m gonna move to Italy!",
                "If I make a typo it’s because my eyes are still full of tears of joy.",
                "I’m lactose intolerant but you know what, I would have this every meal for the rest of my life.",
                "I could tell you what was in that pizza sauce; 1 cup of love, 2 cups of heaven and a dash of enlightenment that gave me the final push to quit drinking.",
                "I used to say the best day of my life was when my daughter was born, not anymore."
            }
        }
    };

    private void Awake()
    {
        var score = ServiceLocator.Get<GameManager>().GetScore();
        _textStats.text = "Score: " + score.ToString() + "\nKill Count: " + ServiceLocator.Get<GameManager>().GetKillCount();
        ServiceLocator.Get<GameManager>().ResetScore();
        int starNum = 0;

        var sceneName = ServiceLocator.Get<GameManager>().GetRepalyScene();
        if (score >= 50)
        {
            var level = GetLevelIndex(sceneName);
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

        DisplayQuote(starNum); // Display the quote and image after determining the star count

        foreach (var startworth in _starsWorths)
        {       
            int moneyValue = 0;
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

        var deathDialogue = GetComponent<DeathDialogue>();
        if (sceneName == "MainScene")
        {
            deathDialogue.EnterDialogueMode(starNum);
        }

    }

    private int GetLevelIndex(string name)
    {
        if (name == "MainScene")
        {
            return 0;
        }

        string indexString = name.Replace("Level_", "");
        int level = 0;

        if (int.TryParse(indexString, out level))
        {
            return level;
        }

        Debug.LogError("No level found");
        return 0;
    }

    private void DisplayQuote(int starNum)
    {
        if (quotesByStars.TryGetValue(starNum, out var quotes))
        {
            var randomQuote = quotes[Random.Range(0, quotes.Count)];
            _quoteDisplay.text = randomQuote;
        }
        else
        {
            Debug.LogWarning("No quotes available for starNum: " + starNum);
        }

        // Randomly activate one of the images
        foreach (var image in _images)
        {
            image.SetActive(false);
        }
        var randomImage = _images[Random.Range(0, _images.Length)];
        randomImage.SetActive(true);
    }
}

