using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject AIPrefabs;
    public SceneControl sc;
    public int money = 0;
    private float spawnTime = 0;
    public float RageValue = 0.0f;
    private float rageTime = 0;
    public bool rageMode = false;
    public Text moneycounting;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        spawnTime += Time.deltaTime;
        if(spawnTime > 5 && rageMode == false) 
        {
            Vector2 spawnPos = TileManager.Instance.requestEntrancePos();
            Instantiate(AIPrefabs, spawnPos, Quaternion.identity);
            spawnTime = 0;
        }
        if(RageValue >=100 && rageTime <= 15)
        {
            rageMode = true;
            spawnTime = 0;
            rageTime += Time.deltaTime;
        }
        else if(rageMode && rageTime >= 15)
        {
            RageValue = 0;
            rageMode = false;
            rageTime = 0;
        }
        moneycounting.text = "X " + money.ToString();

        if (money>=100)
        {
            sc.switchToWinScene();
        }
        Debug.Log(RageValue);
    }
}
