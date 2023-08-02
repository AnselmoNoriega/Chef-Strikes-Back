using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private Item foodItem;
    [SerializeField]
    private Item weaponItem;

    public bool AddItem(Item item)
    {
        if(item.tag == "Food" && foodItem == null)
        {
            foodItem = item; 
            item.transform.SetParent(transform);
            item.transform.localPosition = new Vector2(0, 0.1f);
            return true;
        }
        else if(item.tag == "Weapon" && weaponItem == null)
        {
            weaponItem = item;
            item.transform.SetParent(transform);
            item.transform.localPosition = new Vector2(0, 0.1f);
            return true;
        }

        return false;
    }

    public Item GetFoodItem()
    {
       return foodItem;
    }

    public void ThrowFood(Vector2 velocity, Vector2 negativeAcceleration, float time)
    {
        foodItem.Throw(velocity, negativeAcceleration, time);
        foodItem.transform.parent = null;
        foodItem = null;
    }
}
