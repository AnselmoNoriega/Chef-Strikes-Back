using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    Money money;

    Money DropItem()
    {
        return money;
    }
    public void InstantiateLoot(Vector2 spawnPos)
    {
        GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPos, Quaternion.identity);
        lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItemPrefab.moneySprite;
    }
}
