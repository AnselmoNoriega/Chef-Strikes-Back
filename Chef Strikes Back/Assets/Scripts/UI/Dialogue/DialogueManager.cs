using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;

    [SerializeField] private TextMeshProUGUI dialogueText;

    

    private Story currentStory;

    public bool dialogueMode;

    public bool dialogueIsPlaying;

    private static DialogueManager instance;

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }
        
    }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;

        dialogueIsPlaying = true;
        dialoguePanel.SetActive(false);
        dialogueMode = true;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }
    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private void ExitDialogueMode() 
    {
        dialogueIsPlaying = false;    
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        dialogueMode = false;
    }

    public void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
        }
        else
        {
            ExitDialogueMode();
        }
    }
}
