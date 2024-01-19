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

    private float _badAiSpawntimer;
    private float _copSpawntimer;
    public void Initialize()
    {
        _player = ServiceLocator.Get<Player>();
        //SpawnCustomer();
        SpawnCops();
    }

    private void Update()
    {
        _countToSpawn += Time.deltaTime;

        if (_countToSpawn >= spawnTime)
        {
            //SpawnCustomer();
            //SpawnCops();
            _countToSpawn = 0;
        }
        WantedSystem();
    }

    private void SpawnCustomer()
    {
        Vector2 spawnPos = ServiceLocator.Get<AIManager>().ExitPosition();
        Instantiate(AIPrefabs, spawnPos, Quaternion.identity);
    }
    private void SpawnBadCustomer()
    {
        Vector2 spawnPos = ServiceLocator.Get<AIManager>().ExitPosition();
        GameObject customer = Instantiate(AIPrefabs, spawnPos, Quaternion.identity);
        customer.GetComponent<AI>().ChangeState(AIState.Rage);
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
    private void WantedSystem()
    {
        var Killscount = ServiceLocator.Get<Player>().GetKillsCount();
        if (Killscount <= 4)
        {
            //Starts = 0;
        }
        else if (Killscount <= 10)
        {
            //Stars = 1;
            _badAiSpawntimer = 5.0f;
            _badAiSpawntimer -= Time.deltaTime;
            if (_badAiSpawntimer <= 0)
            {
                //Spawn 1 BadAi
                SpawnBadCustomer();
                _badAiSpawntimer = 5.0f;
            }
        }
        else if (Killscount <= 20)
        {
            //star 2
            _badAiSpawntimer = 3.0f;
            _badAiSpawntimer -= Time.deltaTime;
            if (_badAiSpawntimer <= 0)
            {
                //Spawn 2 BadAi
                SpawnBadCustomer();
                SpawnBadCustomer();
                _badAiSpawntimer = 3.0f;
            }

        }
        else if (Killscount <= 30)
        {
            //Stars = 3;
            _badAiSpawntimer = 3.0f;
            _copSpawntimer = 5.0f;
            _badAiSpawntimer -= Time.deltaTime;
            _copSpawntimer -= Time.deltaTime;
            if (_badAiSpawntimer <= 0)
            {
                //Spawn 2 BadAi
                SpawnBadCustomer();
                SpawnBadCustomer();
                _badAiSpawntimer = 3.0f;
            }
            if (_copSpawntimer <= 0)
            {
                SpawnCops();
                _copSpawntimer = 5.0f;
            }
        }
        else if (Killscount <= 50)
        {
            //Stars = 4;
            _badAiSpawntimer = 3.0f;
            _copSpawntimer = 3.0f;
            _badAiSpawntimer -= Time.deltaTime;
            _copSpawntimer -= Time.deltaTime;
            if (_badAiSpawntimer <= 0)
            {
                //Spawn 2 BadAi
                SpawnBadCustomer();
                SpawnBadCustomer();
                _badAiSpawntimer = 3.0f;
            }
            if (_copSpawntimer <= 0)
            {
                SpawnCops();
                _copSpawntimer = 3.0f;
            }
        }
        else if (Killscount >= 50)
        {
            //Stars = 5;
            _badAiSpawntimer = 3.0f;
            _copSpawntimer = 3.0f;
            _badAiSpawntimer -= Time.deltaTime;
            _copSpawntimer -= Time.deltaTime;
            if (_badAiSpawntimer <= 0)
            {
                //Spawn 3 BadAi
                SpawnBadCustomer();
                SpawnBadCustomer();
                SpawnBadCustomer();
                _badAiSpawntimer = 3.0f;
            }
            if (_copSpawntimer <= 0)
            {
                SpawnCops();
                SpawnCops();
                _copSpawntimer = 5.0f;
            }
        }

    }
}
