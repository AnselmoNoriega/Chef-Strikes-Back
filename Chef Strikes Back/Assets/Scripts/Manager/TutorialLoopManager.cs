using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLoopManager : MonoBehaviour
{
    [SerializeField] private GameObject _customerPrefabs;
    [SerializeField] public List<GameObject> FocusPositions;
    bool isConversation;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    private void Awake()
    {
        Initialize();
        
    }

    private void Start()
    {
        EnterConversation(inkJSON);
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
        isConversation = true;
    }
    private void SpawnCustomer()
    {
        Vector2 spawnPos = ServiceLocator.Get<AIManager>().ExitPosition();
        Instantiate(_customerPrefabs, spawnPos, Quaternion.identity);
    }

    private void EnterConversation(TextAsset _inkJSON)
    {
        DialogueManager.GetInstance().EnterDialogueMode(_inkJSON);
    }

}
