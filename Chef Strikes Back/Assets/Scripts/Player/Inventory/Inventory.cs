using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Attachments")]
    [SerializeField] private Item _foodItem;
    [SerializeField] private Slider _length;
    [SerializeField] private GameObject _pointer;
    [SerializeField] private Transform _foodPosition;

    [Header("Variables")]
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _playerForce;
    [SerializeField] private float _distanceMultiplier;
    [SerializeField] private Vector2 _pointerSize;

    private Player _player;
    private SpriteRenderer _pointerImage;
    private bool _isLaunchingFood;

    private void Awake()
    {
        _pointerImage = _pointer.GetComponent<SpriteRenderer>();
        _player = GetComponent<Player>();
        _isLaunchingFood = false;
        _length.value = 0.0f;
    }

    private void Update()
    {
        if (_isLaunchingFood)
        {
            _length.transform.position = transform.localPosition + _offset;
            _length.value += Time.deltaTime * _distanceMultiplier;
            PointerMovement();
        }
    }

    public void AddItem(Item item)
    {
        _foodItem = item;
        item.IsPickable = false;
        item.transform.SetParent(_foodPosition);
        item.transform.localPosition = Vector2.zero;
    }

    public Item GetFoodItem()
    {
        return _foodItem;
    }

    public void PrepareToThrowFood(InputAction pos)
    {
        _pointer.SetActive(true);
        _isLaunchingFood = true;
        _length.gameObject.SetActive(true);
        _length.value = 0.0f;
    }

    public void ThrowFood(Vector2 direction)
    {
        _pointer.SetActive(false);
        SetEquation2Throw(direction);
        _foodItem.CollidersState(true);
        _foodItem.transform.parent = null;
        _foodItem = null;
        _isLaunchingFood = false;
        _length.gameObject.SetActive(false);
    }

    private void SetEquation2Throw(Vector2 direction)
    {
        direction = _pointer.transform.up.normalized;
        if (direction == Vector2.zero)
        {
            _foodItem.Throw(Vector2.zero, Vector2.zero, 0);
            return;
        }

        Vector3 mousePos = direction * _length.value;

        var strength = mousePos * _playerForce;

        float velocity = math.sqrt(math.pow(strength.x, 2) + math.pow(strength.y, 2));
        float distance = math.sqrt(math.pow(mousePos.x, 2) + math.pow(mousePos.y, 2));

        float acceleration = (math.pow(velocity, 2)) / (2 * distance);
        Vector2 negativeAcceleration = (-acceleration * mousePos / distance);

        _foodItem.Throw(strength, negativeAcceleration, velocity / acceleration);
    }

    private void PointerMovement()
    {
        var angle = Math.Atan2(-_player.LookingDirection.x, _player.LookingDirection.y) * Mathf.Rad2Deg;
        _pointer.transform.rotation = Quaternion.Euler(0.0f, 0.0f, (float)angle);
        _pointerSize.y = math.min(1.0f, math.max(_length.value / 5, 0.27f));
        _pointerImage.size = _pointerSize;
    }
}
