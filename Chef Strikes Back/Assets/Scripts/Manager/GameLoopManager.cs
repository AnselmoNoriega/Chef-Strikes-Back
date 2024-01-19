using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameLoopManager : MonoBehaviour
{
    [SerializeField] private float spawnTime;
    [SerializeField] private GameObject AIPrefabs;
    [SerializeField] private GameObject CopsPrefabs;

    private List<GameObject> _AIPool = new();
    private Player _player;
    private float _countToSpawn = 0;

    public void Initialize()
    {
        _player = ServiceLocator.Get<Player>();
        SpawnCustomer();
        //SpawnCops();
    }

    private void Update()
    {
        _countToSpawn += Time.deltaTime;

        if (_countToSpawn >= spawnTime)
        {
            SpawnCustomer();
            //SpawnCops();
            _countToSpawn = 0;
        }
    }

    private void SpawnCustomer()
    {
        Vector2 spawnPos = ServiceLocator.Get<AIManager>().ExitPosition();
        Instantiate(AIPrefabs, spawnPos, Quaternion.identity);
    }

    private void SpawnCops()
    {
        Vector2 spawnPos = ServiceLocator.Get<AIManager>().ExitPosition();
        Instantiate(CopsPrefabs, spawnPos, Quaternion.identity);
    }


    public void ChangeSpawnTime(int time)
    {
        spawnTime = time;
    }


    public void RemoveAI(GameObject ai)
    {
        _AIPool.Remove(ai);
    }

    public void AddBadAI(GameObject ai)
    {
        _AIPool.Add(ai);
    }
}
