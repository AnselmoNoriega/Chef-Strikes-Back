using System.Collections.Generic;
using UnityEngine;

public class TutorialLoopManager : MonoBehaviour
{
    [Header("Game Loop")]
    [SerializeField] private GameObject _customerPrefabs;

    [SerializeField] public List<GameObject> FocusPositions;

    [Header("Ink Text")]
    [SerializeField] private DialogueManager _dialogueManager;
    [SerializeField] private List<TextAsset> inkJSON;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {

    }

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

    private void SpawnCustomer()
    {
        Vector2 spawnPos = ServiceLocator.Get<AIManager>().ExitPosition();
        Instantiate(_customerPrefabs, spawnPos, Quaternion.identity);
    }

    private void EnterConversation(TextAsset _inkJSON)
    {
        _dialogueManager.EnterDialogueMode(_inkJSON);
    }

}
