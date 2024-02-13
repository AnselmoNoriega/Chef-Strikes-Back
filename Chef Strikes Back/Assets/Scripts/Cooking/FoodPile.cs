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

    private void Update()
    {
        if (Vector2.Distance(_playerTransform.position, transform.position) <= 1.0f)
        {
            var distance = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position);
            ActivateLight(distance <= 1.0f);
        }
        else
        {
            ActivateLight(false);
        }
    }

    public GameObject Hit()
    {
        ServiceLocator.Get<AudioManager>().PlaySource("cut");
        return Instantiate(_foodItem, transform.position, Quaternion.identity);
    }

    public void ActivateLight(bool active)
    {
        if (active != _light.enabled)
        {
            _light.enabled = active;
        }
    }

}
