using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLoopManager : MonoBehaviour
{
    [SerializeField] private GameObject _customerPrefabs;
    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        //flow of tutorial
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

}
