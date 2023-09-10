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

    
}
