using System.Collections.Generic;
using UnityEngine;

public class TutorialLoopManager : MonoBehaviour
{
    [SerializeField] private TutorialCameraManager _tutorialCameraManager;
    [SerializeField] private List<GameObject> _focusPositions;

    [Header("Ink Text")]
    [SerializeField] private List<TextAsset> inkJSON;

    private int _tutorialState = 0;

    private void Start()
    {
        EnterConversation(inkJSON[_tutorialState]);
    }

    private void Initialize()
    {
        //initialize variable
    }

    private void Update()
    {
        //flow of tutorial
        //dialogue

        //camera
        //spawn a Customer
    }

    private void EnterConversation(TextAsset _inkJSON)
    {
        ServiceLocator.Get<DialogueManager>().EnterDialogueMode(_inkJSON);
    }

    public void FinishCurrentDialogue()
    {
        ++_tutorialState;

        switch (_tutorialState)
        {
            case 1:
                {
                    _tutorialCameraManager.
                }
                break;
        }
    }

}
