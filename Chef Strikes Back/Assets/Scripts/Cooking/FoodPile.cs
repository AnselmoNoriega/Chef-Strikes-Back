using UnityEngine;

public class FoodPile : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    public Player player;
    [SerializeField]
    private GameObject foodItem;
    public GameObject meatItem;
    public GameObject bunItem;
    public GameObject lettuceItem;
    [SerializeField]
    private float throwStrength;
    [SerializeField]
    private int startHealth;
    private int health;
    [SerializeField]
    public AudioClip meatSound;
    public AudioClip bunSound;
    public AudioClip lettuceSound;
    public AudioSource source;

    private void Start()
    {
        health = startHealth;
    }

    private void Update()
    {
        if (foodItem == meatItem)
        {
            source.clip = meatSound;
            
        }

        if (foodItem == bunItem)
        {
            source.clip = bunSound;

        }

        if (foodItem == lettuceItem)
        {
            source.clip = lettuceSound;

        }
    }

    public void Hit(int amt)
    {
        health -= amt;

        if(health <= 0)
        {
            source.Play();
            Vector2 direction = (target.position - transform.position).normalized;
            var randAngle = Random.Range(-45, 45);

            var item = Instantiate(foodItem, transform.position, Quaternion.identity);

            var strength = Quaternion.Euler(0, 0, randAngle) * direction * throwStrength;

            item.GetComponent<Item>().Throw(strength, -strength/ 0.5f, 0.5f);
            health = startHealth;
        }
    }

    
}
