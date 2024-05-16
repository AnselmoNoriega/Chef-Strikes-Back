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

    private int _storyIdx = 0;
    private int _focusPosIdx = 0;

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
            ServiceLocator.Get<DialogueManager>().EnterDialogueModeBool(inkJSONFoodThrow, "ingredientInHand", timeUp);
            inkJSONFoodThrow = null;
        }
    }

    public void TriggerCauldronEvent(bool isPizza)
    {
        if (inkJSONCauldronEvent)
        {
            ServiceLocator.Get<DialogueManager>().EnterDialogueModeBool(inkJSONCauldronEvent, "pizzaMade", isPizza);
            if (isPizza)
            {
                inkJSONCauldronEvent = null;
            }
        }
    }

    public void PickUpEvent()
    {
        ServiceLocator.Get<DialogueManager>().EnterDialogueMode(inkJSONPickingUp, false);
    }

}
