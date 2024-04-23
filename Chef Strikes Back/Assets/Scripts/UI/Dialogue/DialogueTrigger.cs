using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    bool isConversation;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private void Awake()
    {
        isConversation = true;
    }
    void Update()
    {
        if (isConversation)
        {
            //ServiceLocator.Get<DialogueManager>().EnterDialogueMode(inkJSON);
            isConversation = false;
        }
    }
}
