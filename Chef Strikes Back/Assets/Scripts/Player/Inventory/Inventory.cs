using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Item foodItem;
    [SerializeField] private Item weaponItem;
    [SerializeField] private float playerForce;
    [SerializeField] private float distanceMultiplier;
    [SerializeField] private Slider length;
    [SerializeField] private Vector3 offset;
    [SerializeField] private GameObject pointer;
    private Vector3 offsetPointer;

    private SpriteRenderer _pointerImage;

    private Vector3 pointingDirection;
    private Vector2 _pointerSize;

    private bool isLaunchingFood;
    private InputAction targetAngle;

    private void Start()
    {
        _pointerSize = new Vector2(1.0f, 0.27f);
        isLaunchingFood = false;
        length.value = 0.0f;
        offsetPointer = new Vector3(0, 0.35f, 0);
        _pointerImage = pointer.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isLaunchingFood)
        {
            length.transform.position = transform.localPosition + offset;
            length.value += Time.deltaTime * distanceMultiplier;
            PointerMovement();
        }
    }

    public void AddItem(Item item)
    {
        foodItem = item;
        item.IsPickable = false;
        item.transform.SetParent(transform);
        item.transform.localPosition = new Vector2(0, 0.7f);
    }

    public Item GetFoodItem()
    {
        return foodItem;
    }

    public void PrepareToThrowFood(InputAction pos)
    {
        pointer.SetActive(true);
        targetAngle = pos;
        isLaunchingFood = true;
        length.gameObject.SetActive(true);
        length.value = 0.0f;
    }

    public void ThrowFood(Vector2 direction)
    {
        pointer.SetActive(false);
        SetEquation2Throw(direction);
        foodItem.CollidersState(true);
        foodItem.transform.parent = null;
        foodItem = null;
        targetAngle = null;
        isLaunchingFood = false;
        length.gameObject.SetActive(false);
    }

    private void SetEquation2Throw(Vector2 direction)
    {
        direction = pointer.transform.up.normalized;
        if (direction == Vector2.zero)
        {
            foodItem.Throw(Vector2.zero, Vector2.zero, 0);
            return;
        }

        Vector3 mousePos = direction * length.value;

        var strength = mousePos * playerForce;

        float velocity = math.sqrt(math.pow(strength.x, 2) + math.pow(strength.y, 2));
        float distance = math.sqrt(math.pow(mousePos.x, 2) + math.pow(mousePos.y, 2));

        float acceleration = (math.pow(velocity, 2)) / (2 * distance);
        Vector2 negativeAcceleration = (-acceleration * mousePos / distance);

        foodItem.Throw(strength, negativeAcceleration, velocity / acceleration);
    }

    private void PointerMovement()
    {
        if (targetAngle.ReadValue<Vector2>().magnitude > 10.0f)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(targetAngle.ReadValue<Vector2>());
            pointingDirection = (mousePos - (transform.position + offsetPointer));
        }
        else if(targetAngle.ReadValue<Vector2>().magnitude > 0.0f)
        {
            pointingDirection = targetAngle.ReadValue<Vector2>();
        }

        var angle = (float)Math.Atan2(-pointingDirection.x, pointingDirection.y) * Mathf.Rad2Deg;
        pointer.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        _pointerSize.y = math.min(1.0f, math.max(length.value / 5, 0.27f));
        _pointerImage.size = _pointerSize;
    }
}
