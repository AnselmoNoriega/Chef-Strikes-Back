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

    private int _storyIdx = 0;
    private int _focusPosIdx = 0;

    private void Start()
    {
        EnterConversation();
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

    public void EnterConversation()
    {
        ServiceLocator.Get<DialogueManager>().EnterDialogueMode(inkJSON[_storyIdx++]);
    }

    public void ChangeFocusTarget()
    {
        switch (_focusPosIdx)
        {
            case 0:
                {
                    SpawnCustomer();
                    break;
                }
        }

        if (_focusPositions.Count > _focusPosIdx)
        {
            _tutorialCameraManager.ChangeTarget(_focusPositions[_focusPosIdx++]);
        }
    }

    private void SpawnCustomer()
    {
        Vector2 spawnPos = ServiceLocator.Get<AIManager>().ExitPosition();
        var customer = Instantiate(_aiPrefab, spawnPos, Quaternion.identity);
        _focusPositions[_focusPosIdx] = customer.transform;
    }

}
