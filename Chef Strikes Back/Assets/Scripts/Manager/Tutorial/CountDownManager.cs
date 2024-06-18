using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDownManager : MonoBehaviour
{
    [Header ("Canvas Prop")]
    [SerializeField] private TMP_Text _countDownText;
    [SerializeField] public GameObject _countDownPanel;
    [SerializeField] private string _endMessage;
    private int _countDownCount = 3;
    public bool GameStart = false;


    public void StartCountDown()
    {
        _countDownPanel.SetActive(true);
        ServiceLocator.Get<Player>().shouldNotMove = true;
        StartCoroutine(CountDown());
    }   
    
    private IEnumerator CountDown()
    {
        if(_countDownCount > 0)
        {
            _countDownText.text = _countDownCount.ToString();
        }
        else
        {
            _countDownText.text = _endMessage;
        }
        yield return new WaitForSeconds(1f);
        _countDownCount--;
        if(_countDownCount >=0)
        {
            StartCoroutine(CountDown());
        }
        else
        {
            _countDownPanel.SetActive(false);
            ServiceLocator.Get<TutorialTimer>().SetTimeState(true);
            ServiceLocator.Get<AIManager>().GetComponent<AISupportManager>().SetAllChair();
            var glm = ServiceLocator.Get<GameLoopManager>();
            ServiceLocator.Get<TutorialLoopManager>().PlayerShouldMove();
            glm.enabled = true;
            glm.Initialize();
        }
    }

    public void StartGame()
    {
        _countDownPanel.SetActive(true);
        ServiceLocator.Get<Player>().shouldNotMove = true;
        StartCoroutine(LevelCountDown());
    }

    private IEnumerator LevelCountDown()
    {
        if (_countDownCount > 0)
        {
            _countDownText.text = _countDownCount.ToString();
        }
        else
        {
            _countDownText.text = _endMessage;
        }
        yield return new WaitForSeconds(1f);
        _countDownCount--;
        if (_countDownCount >= 0)
        {
            StartCoroutine(LevelCountDown());
        }
        else
        {
            _countDownPanel.SetActive(false);
            var glm = ServiceLocator.Get<GameLoopManager>();
            ServiceLocator.Get<Player>().shouldNotMove = false;
            ServiceLocator.Get<AudioManager>().BGMforScenes();
            glm.enabled = true;
            ServiceLocator.Get<LevelTimer>().SetTimeState(true);
            glm.Initialize();
        }
    }
}
