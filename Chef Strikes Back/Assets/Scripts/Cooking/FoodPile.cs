using UnityEngine;

public class FoodPile : MonoBehaviour
{
    [SerializeField]
    private Transform target;
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
            Vector2 direction = (target.position - transform.position).normalized;
            var randAngle = Random.Range(-45, 45);

            var item = Instantiate(foodItem, transform.position, Quaternion.identity);

            var strength = Quaternion.Euler(0, 0, randAngle) * direction * throwStrength;

            item.GetComponent<Item>().Throw(strength, -strength/ 0.5f, 0.5f);
            health = startHealth;
        }
    }
}
