using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private Item foodItem;
    [SerializeField]
    private Item weaponItem;
    [SerializeField]
    private float playerForce;
    [SerializeField]
    private float distanceMultiplier;
    [SerializeField]
    private Slider length;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private GameObject pointer;
    private Vector3 offsetPointer;

    private bool isLaunchingFood;
    private InputAction targetMouse;

    private void Start()
    {
        isLaunchingFood = false;
        length.value = 0.0f;
        offsetPointer = new Vector3(0, 0.35f, 0);
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
        item.isPickable = false;
        item.transform.SetParent(transform);
        item.transform.localPosition = new Vector2(0, 0.7f);
    }

    public Item GetFoodItem()
    {
        return foodItem;
    }

    public void PrepareToThrowFood(InputAction mouse)
    {
        pointer.SetActive(true);
        targetMouse = mouse;
        isLaunchingFood = true;
        length.gameObject.SetActive(true);
        length.value = 0.0f;
    }

    public void ThrowFood(Vector2 direction)
    {
        pointer.SetActive(false);
        SetEquation2Throw(direction);
        foodItem.transform.parent = null;
        foodItem = null;
        targetMouse = null;
        isLaunchingFood = false;
        length.gameObject.SetActive(false);
    }

    private void SetEquation2Throw(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            foodItem.Throw(Vector2.zero, Vector2.zero, 0);
            return;
        }

        Vector3 mousePos = direction * length.value;

        var strength = mousePos * playerForce;

        float velocity = (float)Math.Sqrt(Math.Pow(strength.x, 2f) + Math.Pow(strength.y, 2f));
        float distance = (float)Math.Sqrt(Math.Pow(mousePos.x, 2f) + Math.Pow(mousePos.y, 2f));

        float acceleration = (float)(Math.Pow(velocity, 2)) / (2 * distance);
        Vector2 negativeAcceleration = (-acceleration * mousePos / distance);

        foodItem.Throw(strength, negativeAcceleration, velocity / acceleration);
    }

    private void PointerMovement()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(targetMouse.ReadValue<Vector2>());
        var dir = (mousePos - (transform.position + offsetPointer));
        var angle = (float)Math.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;
        pointer.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        pointer.transform.localScale = new Vector3(0.5f, length.value, 1.0f);
    }
}
