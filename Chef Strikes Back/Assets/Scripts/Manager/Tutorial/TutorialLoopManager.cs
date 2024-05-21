using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
struct DictionaryField<KeyType, ValueType>
{
    public KeyType Name;
    public ValueType Value;
}

public class TutorialLoopManager : MonoBehaviour
{
    [SerializeField] private TutorialCameraManager _tutorialCameraManager;
    [SerializeField] private List<ButtonReminder> buttonReminder;
    [SerializeField] private GameObject _aiPrefab;

    [SerializeField] private Player _player;
    private AI _tutorialAI;

    public AIState AiStandState = AIState.Good;
    public int AiChoice = 0;

    [Header("Ink Text")]
    [SerializeField] private List<TextAsset> inkJSON;
    [SerializeField] private List<DictionaryField<string, TextAsset>> inkJSONEvents;
    private Dictionary<string, TextAsset> _eventsDictionary = new();

    private int _storyIdx = 0;
    private int _focusPosIdx = 0;
    public bool TutorialSecondFace = false;
    public bool _multipleSpaghettiesMade = false;

    private void Start()
    {
        EnterConversation();

        foreach(var inkEvent in inkJSONEvents)
        {
            _eventsDictionary.Add(inkEvent.Name, inkEvent.Value);
        }
    }

    public void EnterConversation()
    {
        ServiceLocator.Get<DialogueManager>().EnterDialogueMode(inkJSON[_storyIdx++]);
    }

    public void EndConversation()
    {
        switch (_focusPosIdx)
        {
            case 0:
                {
                    SpawnCustomer();
                    break;
                }
            case 1:
                {
                    _tutorialAI.ChangeState(AIState.Hungry);
                    EnterConversation();
                    _tutorialCameraManager.ChangeTarget(_tutorialAI.OrderBubble[_tutorialAI.ChoiceIndex].transform);
                    break;
                }
            case 2:
                {
                    foreach (var button in buttonReminder)
                    {
                        button.SetHitOn(true);
                    }
                    _tutorialCameraManager.ChangeTarget(_player.transform);
                    _tutorialCameraManager.ZoomIn(-0.6f, -0.6f);
                    break;
                }
            case 4:
                {
                    _tutorialAI.ChangeState(AIState.Hungry);
                    _tutorialCameraManager.ChangeTarget(_player.transform);
                    _tutorialCameraManager.ZoomIn(0.2f, 0.2f);
                    break;
                }
            case 5:
                {
                    _tutorialCameraManager.ZoomIn(-5.0f, -5.0f);
                    ServiceLocator.Get<GameManager>().SetThisLevelSceneName(SceneManager.GetActiveScene().name);
                    ServiceLocator.Get<Player>().shouldNotMove = true;
                    _tutorialAI.ChangeState(AIState.Hungry);
                    _tutorialAI.enabled = true;
                    break;
                }
            case 6:
                {
                    _tutorialCameraManager.ChangeTarget(_player.transform);
                    _tutorialCameraManager.ZoomIn(0.2f, 0.2f);
                    break;
                }
            case 7:
                {
                    _tutorialCameraManager.ZoomIn(0.2f, 0.2f);
                    ServiceLocator.Get<TutorialTimer>().SetTimeState(true);
                    var glm = ServiceLocator.Get<GameLoopManager>();
                    glm.enabled = true;
                    glm.Initialize();
                    break;
                }

        }
        ++_focusPosIdx;
    }

    public void SpawnCustomer(bool shouldPause = true)
    {
        Vector2 spawnPos = ServiceLocator.Get<AIManager>().ExitPosition();
        var customer = Instantiate(_aiPrefab, spawnPos, Quaternion.identity);
        _tutorialCameraManager.ChangeTarget(customer.transform);
        _tutorialAI = customer.GetComponent<AI>();

        EnterConversation();
        ServiceLocator.Get<DialogueManager>().IsPaused = shouldPause;
    }

    public void EnterDialogueEvent(string name, bool triggerExit = false)
    {
        if (_eventsDictionary.ContainsKey(name))
        {
            ServiceLocator.Get<DialogueManager>().EnterDialogueMode(_eventsDictionary[name], triggerExit);
            _eventsDictionary.Remove(name);
        }
    }

    public void CheckIfHolding(bool timeUp)
    {
        if (_eventsDictionary.ContainsKey("FoodThrow"))
        {
            string[] names = new string[] { "ingredientInHand" };
            bool[] bools = new bool[] { timeUp };
            ServiceLocator.Get<DialogueManager>().EnterDialogueModeBool(_eventsDictionary["FoodThrow"], names, bools);
            _eventsDictionary.Remove("FoodThrow");
        }
    }

    public void TriggerCauldronEvent(bool isPizza)
    {
        if (_eventsDictionary.ContainsKey("PizzaMade"))
        {
            string[] names = new string[] { "pizzaMade" };
            bool[] bools = new bool[] { isPizza };
            ServiceLocator.Get<DialogueManager>().EnterDialogueModeBool(_eventsDictionary["PizzaMade"], names, bools);
            if (isPizza)
            {
                _eventsDictionary.Remove("PizzaMade");
            }
        }
    }

    public void TriggerSpaghettiEvent(bool isSpaghetti)
    {
        if (!ServiceLocator.Get<DialogueManager>().dialogueIsPlaying && _eventsDictionary.ContainsKey("SpaghettiMade"))
        {
            string[] names = new string[] { "spaghettiMade", "wrongFoodMadeBefore" };
            bool[] bools = new bool[] { isSpaghetti, _multipleSpaghettiesMade };
            _multipleSpaghettiesMade = true;
            ServiceLocator.Get<DialogueManager>().EnterDialogueModeBool(_eventsDictionary["SpaghettiMade"], names, bools);
            if (isSpaghetti)
            {
                _eventsDictionary.Remove("SpaghettiMade");
            }
        }
    }

}
