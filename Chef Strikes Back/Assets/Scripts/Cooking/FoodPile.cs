using UnityEngine;

public class FoodPile : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private GameObject foodItem;
    [SerializeField]
    private float throwStrength;

    public GameObject Hit()
    {
        return Instantiate(foodItem, transform.position, Quaternion.identity);
    }
}
