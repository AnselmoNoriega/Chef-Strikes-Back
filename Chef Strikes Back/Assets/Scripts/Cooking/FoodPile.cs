using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FoodPile : MonoBehaviour
{
    [SerializeField] private GameObject _foodItem;
    [SerializeField] private float _lightDistance;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public GameObject Hit()
    {
        ServiceLocator.Get<AudioManager>().PlaySource("cut");
        return Instantiate(_foodItem, transform.position, Quaternion.identity);
    }

    public SpriteRenderer GetOutline()
    {
        return _spriteRenderer;
    }

}
