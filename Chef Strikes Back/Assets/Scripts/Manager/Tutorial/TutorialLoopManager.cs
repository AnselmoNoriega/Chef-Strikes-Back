using System.Collections.Generic;
using UnityEngine;

public class TutorialLoopManager : MonoBehaviour
{

    [SerializeField] public List<GameObject> FocusPositions;

    [Header("Ink Text")]
    [SerializeField] private List<TextAsset> inkJSON;

    
    private void Update()
    {
        //flow of tutorial
        //dialogue
        
        //camera
        //spawn a Customer
    }

    private void Initialize()
    {
        //initialize variable
    }

    private void EnterConversation(TextAsset _inkJSON)
    {
        ServiceLocator.Get<DialogueManager>().EnterDialogueMode(_inkJSON);
    }

}
