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
        _player = ServiceLocator.Get<Player>();
        _aiManager = ServiceLocator.Get<AIManager>();
        SpawnCustomer();
    }

    private void Update()
    {
        _countToSpawn += Time.deltaTime;

        SpawnBadAIWithinTime();
        SpawnTheCopWithinTime();
        WantedStarSpawn();

        if (_countToSpawn >= spawnTime)
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
            Instantiate(AIPrefabs, spawnPos, Quaternion.identity);
        }
    }
    private void SpawnBadCustomer()
    {
        Vector2 spawnPos = ServiceLocator.Get<AIManager>().BadAiEnterPosition();
        AiStandState = AIState.Rage;
        Instantiate(AIPrefabs, spawnPos, Quaternion.identity);
        AiStandState = AIState.Good;
    }
    private void SpawnCops()
    {
        Vector2 spawnPos = ServiceLocator.Get<AIManager>().CopEnterPosition();
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

    public void WantedSystem()
    {
        var Killscount = ServiceLocator.Get<Player>().GetKillsCount();

        if (Killscount == 1)
        {
            _badAiTime2Spawn = 0.0f;
            _badAiCount = 0;
            _stars = 1;
        }
        else if (Killscount == 5)
        {
            _badAiTime2Spawn = 5.0f;
            _badAiCount = 1;
            _stars = 2;
        }
        else if (Killscount == 15)
        {
            _badAiTime2Spawn = 5.0f;
            _badAiCount = 1;
            _copTime2Spawn = 10.0f;
            _copCount = 1;
            _stars = 3;
        }
        else if (Killscount == 25)
        {
            _badAiTime2Spawn = 3.0f;
            _badAiCount = 1;
            _copTime2Spawn = 5.0f;
            _copCount = 1;
            _stars = 4;
        }
        else if (Killscount == 50)
        {
            _badAiTime2Spawn = 3.0f;
            _badAiCount = 2;
            _copTime2Spawn = 5.0f;
            _copCount = 1;
            _stars = 5;
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
