using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject AIPrefabs;
    public int money = 0;
    private float spawnTime = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        spawnTime += Time.deltaTime;
        if(spawnTime > 5) 
        {
            Vector2 spawnPos = TileManager.Instance.requestEntrancePos();
            Instantiate(AIPrefabs, spawnPos, Quaternion.identity);
            spawnTime = 0;
        }
        
    }
}
