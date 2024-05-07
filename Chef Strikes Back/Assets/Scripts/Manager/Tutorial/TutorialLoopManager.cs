using System.Collections.Generic;
using UnityEngine;

public class TutorialLoopManager : MonoBehaviour
{
    [SerializeField] private TutorialCameraManager _tutorialCameraManager;
    [SerializeField] private List<Transform> _focusPositions;
    [SerializeField] private GameObject _aiPrefab;

    public AIState AiStandState = AIState.Good;

    [Header("Ink Text")]
    [SerializeField] private List<TextAsset> inkJSON;

    private int _tutorialState = 0;

    private void Start()
    {
        EnterConversation(0);
    }

    private void Initialize()
    {

    }

    private void Update()
    {
        //flow of tutorial
        //dialogue

        //camera
        //spawn a Customer
    }

    public void EnterConversation(int idx)
    {
        ServiceLocator.Get<DialogueManager>().EnterDialogueMode(inkJSON[idx]);
        ChangeFocusTarget();
    }

    public void ChangeFocusTarget()
    {
        switch (_tutorialState)
        {
            case 1:
            case 3:
                {
                    SpawnCustomer();
                    break;
                }
        }

        if (_focusPositions.Count > _tutorialState)
        {
            _tutorialCameraManager.ChangeTarget(_focusPositions[_tutorialState]);
        }

        ++_tutorialState;
    }
    private void SpawnCustomer()
    {
        Vector2 spawnPos = ServiceLocator.Get<AIManager>().ExitPosition();
        var customer = Instantiate(_aiPrefab, spawnPos, Quaternion.identity);
        _focusPositions[_tutorialState] = customer.transform;
    }

}
