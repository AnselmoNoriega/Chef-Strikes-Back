using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct WantedSpawner
{
    public float KillCount;
    public int Stars;
    public float BadAiSpawnTimer;
    public float CopAiSpawnTimer;
    public int CopSpwanCount;
    public int BadAiSpwanCount;
}

public class GameLoopManager : MonoBehaviour
{
    public float CustomerFoodWaitingTime;
    [SerializeField] private float _spawnTime;
    [SerializeField] private GameObject _customerPrefabs;
    [SerializeField] private GameObject _copsPrefabs;
    [SerializeField] private List<WantedSpawner> _wantedSystemTimer;

    private float _countToSpawn = 0;

    private float _badAiSpawntimer = 0.0f;
    private float _copSpawntimer = 0.0f;

    private int _badAiCount = 0;
    private int _copCount = 0;
    private int _stars = 0;

    private float _badAiTime2Spawn = 0;
    private float _copTime2Spawn = 0;

    public AIState AiStandState = AIState.Good;

    private AIManager _aiManager;
    public void Initialize()
    {
        _aiManager = ServiceLocator.Get<AIManager>();
        ServiceLocator.Get<GameManager>().SetThisLevelSceneName(SceneManager.GetActiveScene().name);
        SpawnCustomer();
    }

    private void Update()
    {
        _countToSpawn += Time.deltaTime;

        SpawnBadAIWithinTime();
        SpawnTheCopWithinTime();
        
        if (_countToSpawn >= _spawnTime)
        {
            SpawnCustomer();
            _countToSpawn = 0;
        }
    }

    private void SpawnCustomer()
    {
        if (_aiManager.GetAvailableChairsCount() > 0)
        {
            Vector2 spawnPos = ServiceLocator.Get<AIManager>().ExitPosition();
            Instantiate(_customerPrefabs, spawnPos, Quaternion.identity);
        }
    }

    
    private void SpawnBadCustomer()
    {
        Vector2 spawnPos = ServiceLocator.Get<AIManager>().BadAiEnterPosition();
        AiStandState = AIState.Rage;
        Instantiate(_customerPrefabs, spawnPos, Quaternion.identity);
        AiStandState = AIState.Good;
    }
    private void SpawnCops()
    {
        Vector2 spawnPos = ServiceLocator.Get<AIManager>().CopEnterPosition();
        Instantiate(_copsPrefabs, spawnPos, Quaternion.identity);
    }

    public void ChangeSpawnTime(int time)
    {
        _spawnTime = time;
    }

    public void WantedSystem()
    {
        var Killscount = ServiceLocator.Get<Player>().KillCount;

        for (int i = _wantedSystemTimer.Count - 1; i >= 0; --i) 
        {
            if (Killscount >= _wantedSystemTimer[i].KillCount)
            {
                _copTime2Spawn = _wantedSystemTimer[i].CopAiSpawnTimer;
                _badAiTime2Spawn = _wantedSystemTimer[i].BadAiSpawnTimer;
                _stars = _wantedSystemTimer[i].Stars;
                _badAiCount = _wantedSystemTimer[i].BadAiSpwanCount;
                _copCount = _wantedSystemTimer[i].CopSpwanCount;
                WantedStarSpawn();
                return;
            }
        }
    }

    private void SpawnBadAIWithinTime()
    {
        _badAiSpawntimer -= Time.deltaTime;
        if (_badAiSpawntimer <= 0)
        {
            for (int i = 0; i < _badAiCount; ++i)
            {
                SpawnBadCustomer();
            }
            _badAiSpawntimer = _badAiTime2Spawn;
        }
    }
    private void SpawnTheCopWithinTime()
    {
        _copSpawntimer -= Time.deltaTime;
        if (_copSpawntimer <= 0)
        {
            for (int i = 0; i < _copCount; ++i)
            {
                SpawnCops();
            }
            _copSpawntimer = _copTime2Spawn;
        }
    }

    private void WantedStarSpawn()
    {
        ServiceLocator.Get<CanvasManager>().ActivateStars(_stars);
    }
}
