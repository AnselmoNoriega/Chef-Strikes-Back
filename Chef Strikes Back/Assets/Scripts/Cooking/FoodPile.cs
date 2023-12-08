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
        ServiceLocator.Get<AudioManager>().PlaySource("cut");
        return Instantiate(foodItem, transform.position, Quaternion.identity);
    }
}
