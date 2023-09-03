using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class FoodPile : MonoBehaviour
{
    [SerializeField]
    private GameObject foodItem;
    [SerializeField]
    private float throwStrength;
    [SerializeField]
    private int startHealth;
    private int health;

    private void Start()
    {
        health = startHealth;
    }

    public void Hit(int amt)
    {
        health -= amt;

        if(health <= 0)
        {
            float x = UnityEngine.Random.Range(-2.0f, 2.0f);
            float y = UnityEngine.Random.Range(-2.0f, 2.0f);

            var item = Instantiate(foodItem, transform.position, Quaternion.identity);

            var strength = new Vector2(x, y) * throwStrength;

            item.GetComponent<Item>().Throw(strength, -strength/ 0.5f, 0.5f);
            health = startHealth;
            Debug.Log(new Vector2(x, y) * throwStrength);
        }
    }
}
