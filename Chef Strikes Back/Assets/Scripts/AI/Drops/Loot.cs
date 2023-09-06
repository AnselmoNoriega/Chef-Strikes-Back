using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "loot", menuName = "Loot")]
public class Loot : ScriptableObject
{
    public Sprite lootSprite;
    public string lootName;
    public int dropChance;

    public Loot(string lootName, int dropChange)
    {
        this.lootName = lootName;
        this.dropChance = dropChange;   
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
<<<<<<< Updated upstream
        if (collider.transform.tag == "Player" && lootName == "Money")
        {
            GameManager.Instance.money += 10;
        }
=======
        GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPos, Quaternion.identity);
        lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItemPrefab.moneySprite;
>>>>>>> Stashed changes
    }
}
