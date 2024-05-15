using Ink.Runtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private TutorialLoopManager _tutorialLoopManager;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;

    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;

    public bool isPaused = false;

    private Story currentStory;

    public bool dialogueMode;

    public bool dialogueIsPlaying;

    private const string SPEAKER_TAG = "speaker";

    private const string PORTRAIT_TAG = "portrait";

    private const string LAYOUT_TAG = "layout";
   
    public void Initialize()
    {
        _tutorialLoopManager = ServiceLocator.Get<TutorialLoopManager>();

        dialogueIsPlaying = true;
        dialoguePanel.SetActive(false);
        dialogueMode = true;
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        ServiceLocator.Get<Player>().shouldNotMove = true;
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
    }

    public void EnterDialogueModeBool(TextAsset inkJSON, string name, bool active)
    {
        ServiceLocator.Get<Player>().shouldNotMove = true;
        currentStory = new Story(inkJSON.text);
        currentStory.variablesState[name] = active;
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
        ServiceLocator.Get<Player>().shouldNotMove = false;
    }

    public void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();

            HandleTags(currentStory.currentTags);
        }
        else
        {
            ExitDialogueMode();
            _tutorialLoopManager.EndConversation();
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if(splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    Debug.Log("portrait=" + tagValue);
                    break;
                case LAYOUT_TAG:
                    Debug.Log("layout=" + tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }

    public void SetPause()
    {
        if(isPaused)
        {
            isPaused = false;
        }
        else
        {
            isPaused = true;
        }
    }
}
