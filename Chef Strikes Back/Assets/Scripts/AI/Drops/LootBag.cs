using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{

    public GameObject droppedItemPrefab;
    public List<Loot> lootList = new List<Loot>();
    
    private Loot GetDroppedItem()
    {
        int randomNumber = Random.Range(1, 101);
        List<Loot> possibleItems = new List<Loot>();
        foreach(Loot item in lootList)
        {
            if(randomNumber <= item.dropChance)
            {
                possibleItems.Add(item);
            }
        }

        return possibleItems[0];
    }

    public void InstantiateLoot(Vector2 spawnPosition)
    {
        Loot droppedItem = GetDroppedItem();
        if(droppedItem != null) 
        {
            GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
            lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.lootSprite;
        }
    }
   
}
