using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject AIPrefabs;
    [SerializeField]
    private Player player;
    public List<GameObject> AIPool;
    public SceneControl sc;
    public int money = 0;
    private float spawnTime = 0;
    public float rageValue = 0.0f;
    public bool rageMode = false;
    public Text moneycounting;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        spawnTime += Time.deltaTime;
        rageValue = player.currentRage;

        if (spawnTime > 5 && rageMode == false)
        {
            Vector2 spawnPos = TileManager.Instance.requestEntrancePos();
            Instantiate(AIPrefabs, spawnPos, Quaternion.identity);
            spawnTime = 0;
        }

        if (rageMode && AIPool.Count == 0)
        {
            player.currentRage = 0;
            rageMode = false;
        }
        else if (rageValue >= 100)
        {
            rageMode = true;
            spawnTime = 0;
        }

        moneycounting.text = "X " + money.ToString();

        if (money >= 100)
        {
            sc.switchToWinScene();
        }
    }
}
