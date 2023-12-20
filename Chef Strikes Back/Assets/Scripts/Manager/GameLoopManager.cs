using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameLoopManager : MonoBehaviour
{
    [SerializeField] private float spawnTime;
    [SerializeField] private GameObject AIPrefabs;

    private List<GameObject> _AIPool = new();
    private Player _player;
    private bool _isInRageMode = false;
    private float _countToSpawn = 0;

    public void Initialize()
    {
        _player = ServiceLocator.Get<Player>();
        SpawnCustomer();
    }

    private void Update()
    {
        _countToSpawn += Time.deltaTime;

        if (_countToSpawn >= spawnTime && _isInRageMode == false)
        {
            SpawnCustomer();
            _countToSpawn = 0;
        }

        if (_isInRageMode && _AIPool.Count == 0)
        {
            ServiceLocator.Get<AIManager>().ResetRandomSpots();
            _player.ExitRageMode();
            _isInRageMode = false;
        }
    }

    private void SpawnCustomer()
    {
        Vector2 spawnPos = ServiceLocator.Get<AIManager>().ExitPosition();
        Instantiate(AIPrefabs, spawnPos, Quaternion.identity);
    }

    public void SetRageMode(bool isRageMode)
    {
        _isInRageMode = isRageMode;
    }

    public void ChangeSpawnTime(int time)
    {
        spawnTime = time;
    }

    public bool IsInRageMode()
    {
        return _isInRageMode;
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
