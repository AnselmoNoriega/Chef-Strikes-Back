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
    [SerializeField]private float spawnTime;
    public float rageValue = 0.0f;
    public bool rageMode = false;
    public Text moneycounting;
    float count = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        
        count += Time.deltaTime;
        rageValue = player.currentRage;

        if (count > spawnTime && rageMode == false)
        {
            Vector2 spawnPos = TileManager.Instance.requestEntrancePos();
            Instantiate(AIPrefabs, spawnPos, Quaternion.identity);
            count = 0;
        }

        if (rageMode && AIPool.Count == 0)
        {
            player.currentRage = 0;
            rageMode = false;
        }
        else if (rageValue >= 100)
        {
            rageMode = true;
            count = 0;
        }


        moneycounting.text = "X " + money.ToString();

        if (money >= 100)
        {
            sc.switchToWinScene();
        }
    }

    public void dealDamage(float damage)
    {
        player.TakeDamage(damage);
    }
}
