using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    [Header("Death Quotes")]
    [SerializeField] private GameObject[] _deathQuoteList;

    [Header("Screen Timer")]
    private DeathDialogue _deathDialogue;
    [SerializeField] private float _screenTimer;
    
    private GameLoader _loader;

    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    public void Initialize()
    {
        Debug.Log("Initializing Death Screen");
        // Get random quote GameObject from the list 
        if(ServiceLocator.Get<GameManager>().GetRepalyScene() == "MainScene")
        {
            _deathDialogue = GetComponent<DeathDialogue>();
            _deathDialogue.EnterDialogueMode(this);
            return;
        }

        var randomQuote = _deathQuoteList[Random.Range(0, _deathQuoteList.Length)];

        // Set random quote GameObject active
        randomQuote.SetActive(true);

        // Start timer to change scenes

        StartCoroutine(GoToStarRatingScreen());
    }

    public void StartExitTimer()
    {
        var randomQuote = _deathQuoteList[Random.Range(0, _deathQuoteList.Length)];

        // Set random quote GameObject active
        randomQuote.SetActive(true);

        StartCoroutine(GoToStarRatingScreen());
    }

    public IEnumerator GoToStarRatingScreen()
    {
        yield return new WaitForSeconds(_screenTimer);

        SceneManager.LoadScene("EndLevel");
    }
}
