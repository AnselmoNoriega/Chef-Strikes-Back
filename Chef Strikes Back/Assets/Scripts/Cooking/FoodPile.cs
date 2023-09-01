using System.Collections;
using System.Collections.Generic;
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
            float x = Random.Range(0.0f, 1.0f);
            float y = Random.Range(0.0f, 1.0f);

            var item = Instantiate(foodItem, transform.position, Quaternion.identity);
            item.GetComponent<Rigidbody2D>().AddForce(new Vector2(x, y) * throwStrength, ForceMode2D.Impulse);
            health = startHealth;
        }
    }
}
