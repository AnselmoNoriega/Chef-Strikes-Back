using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FoodPile : MonoBehaviour
{
    [SerializeField] private GameObject _foodItem;

    private Light2D _light;
    private Transform _playerTransform;

    private void Start()
    {
        _light = GetComponent<Light2D>();
        _playerTransform = ServiceLocator.Get<Player>().transform;
    }

    public GameObject Hit()
    {
        ServiceLocator.Get<AudioManager>().PlaySource("cut");
        return Instantiate(_foodItem, transform.position, Quaternion.identity);
    }

    public void ActivateLight(bool active)
    {
        _light.enabled = active;
    }

    private void OnMouseOver()
    {
        if (Vector2.Distance(_playerTransform.position, transform.position) <= 1)
        {
            ActivateLight(true);
        }
        else
        {
            ActivateLight(false);
        }
    }

    private void OnMouseExit()
    {
        ActivateLight(false);
    }

}
