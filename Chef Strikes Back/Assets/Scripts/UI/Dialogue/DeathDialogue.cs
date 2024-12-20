using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;

public class DeathDialogue : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;

    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;

    [SerializeField] private TextAsset _deathFrace;
    [SerializeField] private TextAsset _starDialogues;
    private DeathScreen _deathScreen;

    private Story currentStory;

    public bool dialogueIsPlaying = false;

    private const string SPEAKER_TAG = "speaker";

    private const string PORTRAIT_TAG = "portrait";

    private const string LAYOUT_TAG = "layout";

    public void EnterDialogueMode(DeathScreen screen)
    {
        _deathScreen = screen;
        currentStory = new Story(_deathFrace.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
    }

    public void EnterDialogueMode(int storyIdx)
    {
        currentStory = new Story(_starDialogues.text);
        currentStory.variablesState["stars"] = storyIdx;
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
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
            if (_deathScreen)
            {
                _deathScreen.StartExitTimer();
            }
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
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
}
