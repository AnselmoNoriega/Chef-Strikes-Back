using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FoodPile : MonoBehaviour
{
    [SerializeField] private GameObject _foodItem;
    [SerializeField] private float _lightDistance;

    private Light2D _light;
    private Transform _playerTransform;

    private void Start()
    {
        _light = GetComponent<Light2D>();
        _playerTransform = ServiceLocator.Get<Player>().transform;
    }

    private void Update()
    {

        if (Vector2.Distance(_playerTransform.position, transform.position) <= _lightDistance)
        {
            CheckObj();
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

    private void CheckObj()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] hits = Physics2D.OverlapCircleAll(mousePos, 0.01f);

        foreach (var hit in hits)
        {
            var foodPile = hit.GetComponent<FoodPile>();
            if (foodPile == this)
            {
                ActivateLight(true);
                return;
            }
        }

        ActivateLight(false);
    }

}
