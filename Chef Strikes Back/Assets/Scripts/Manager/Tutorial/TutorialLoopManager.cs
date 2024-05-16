using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private TextAsset inkJSONFoodThrow;
    [SerializeField] private TextAsset inkJSONPickingUp;
    [SerializeField] private TextAsset inkJSONCauldronEvent;
    [SerializeField] private TextAsset inkJSONSpaghetiEvent;

    private int _storyIdx = 0;
    private int _focusPosIdx = 0;
    public bool TutorialSecondFace = false;
    public bool _multipleSpaghettiesMade = false;

    private void Start()
    {
        EnterConversation();
    }

    private void Update()
    {
        //flow of tutorial
        //dialogue

        //camera
        //spawn a Customer
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
                    break;
                }
            case 3:
                {
                    _tutorialAI.enabled = true;
                    _tutorialAI.ChangeState(AIState.Hungry);
                    _tutorialCameraManager.ChangeTarget(_player.transform);
                    TutorialSecondFace = true;
                    break;
                }

        }
        ++_focusPosIdx;
    }

    public void SpawnCustomer()
    {
        Vector2 spawnPos = ServiceLocator.Get<AIManager>().ExitPosition();
        var customer = Instantiate(_aiPrefab, spawnPos, Quaternion.identity);
        _tutorialCameraManager.ChangeTarget(customer.transform);
        _tutorialAI = customer.GetComponent<AI>();

        EnterConversation();
        ServiceLocator.Get<DialogueManager>().IsPaused = true;
    }

    public void CheckIfHolding(bool timeUp)
    {
        if (inkJSONFoodThrow)
        {
            string[] names = new string[] { "ingredientInHand" };
            bool[] bools = new bool[] { timeUp };
            ServiceLocator.Get<DialogueManager>().EnterDialogueModeBool(inkJSONFoodThrow, names, bools);
            inkJSONFoodThrow = null;
        }
    }

    public void TriggerCauldronEvent(bool isPizza)
    {
        if (inkJSONCauldronEvent)
        {
            string[] names = new string[] { "pizzaMade" };
            bool[] bools = new bool[] { isPizza };
            ServiceLocator.Get<DialogueManager>().EnterDialogueModeBool(inkJSONCauldronEvent, names, bools);
            if (isPizza)
            {
                inkJSONCauldronEvent = null;
            }
        }
    }

    public void TriggerSpaghettiEvent(bool isSpaghetti)
    {
        if (inkJSONSpaghetiEvent)
        {
            string[] names = new string[] { "spaghettiMade", "wrongFoodMadeBefore" };
            bool[] bools = new bool[] { isSpaghetti, _multipleSpaghettiesMade };
            _multipleSpaghettiesMade = true;
            ServiceLocator.Get<DialogueManager>().EnterDialogueModeBool(inkJSONSpaghetiEvent, names, bools);
            if (isSpaghetti)
            {
                inkJSONSpaghetiEvent = null;
            }
        }
    }

    public void PickUpEvent()
    {
        ServiceLocator.Get<DialogueManager>().EnterDialogueMode(inkJSONPickingUp, false);
    }

}
