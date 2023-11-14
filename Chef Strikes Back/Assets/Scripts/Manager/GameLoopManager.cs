using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameLoopManager : MonoBehaviour
{
    public GameObject AIPrefabs;
    [SerializeField]
    private Player player;
    public List<GameObject> AIPool;
    public SceneControl sc;
    public int money = 0;
    [SerializeField]private float spawnTime;
    public float rageValue = 0.0f;
    public bool rageMode = false;
    public TextMeshProUGUI moneycounting;
    private float count;

    public void Initialize()
    {
        count = spawnTime;
    }

    private void Update()
    {
        
        count += Time.deltaTime;
        rageValue = player.currentRage;

        if (count > spawnTime && rageMode == false)
        {
            Vector2 spawnPos = ServiceLocator.Get<TileManager>().requestEntrancePos();
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


        moneycounting.text = "$ " + money.ToString();

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
