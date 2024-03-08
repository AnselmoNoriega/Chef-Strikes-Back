using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Item _foodItem;
    [SerializeField] private Item _weaponItem;
    [SerializeField] private float _playerForce;
    [SerializeField] private float _distanceMultiplier;
    [SerializeField] private Slider _length;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private GameObject _pointer;
    [SerializeField] private Transform _foodPosition;
    private Vector3 _offsetPointer;

    private SpriteRenderer _pointerImage;

    private Vector3 pointingDirection;
    private Vector2 _pointerSize;

    private bool _isLaunchingFood;
    private InputAction _targetAngle;

    private void Start()
    {
        _pointerSize = new Vector2(0.32f, 0.27f);
        _isLaunchingFood = false;
        _length.value = 0.0f;
        _offsetPointer = new Vector3(0, 0.35f, 0);
        _pointerImage = _pointer.GetComponent<SpriteRenderer>();
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
        _targetAngle = pos;
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
        _targetAngle = null;
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
        if (_targetAngle.ReadValue<Vector2>().magnitude > 10.0f)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(_targetAngle.ReadValue<Vector2>());
            pointingDirection = (mousePos - (transform.position + _offsetPointer));
        }
        else if(_targetAngle.ReadValue<Vector2>().magnitude > 0.0f)
        {
            pointingDirection = _targetAngle.ReadValue<Vector2>();
        }

        var angle = (float)Math.Atan2(-pointingDirection.x, pointingDirection.y) * Mathf.Rad2Deg;
        _pointer.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        _pointerSize.y = math.min(1.0f, math.max(_length.value / 5, 0.27f));
        _pointerImage.size = _pointerSize;
    }
}
