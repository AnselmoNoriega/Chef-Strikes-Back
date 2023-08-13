using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private bool isLaunchingFood;
    private InputAction targetMouse;
    private float length;

    private void Start()
    {
        isLaunchingFood = false;
        length = 1f;
    }

    private void Update()
    {
        if(isLaunchingFood)
        {
            length += Time.deltaTime * distanceMultiplier;
        }
    }

    public void AddItem(Item item)
    {
        foodItem = item;
        item.transform.SetParent(transform);
        item.transform.localPosition = new Vector2(0, 0.1f);
    }

    public Item GetFoodItem()
    {
        return foodItem;
    }

    public void PrepareToThrowFood(InputAction mouse)
    {
        targetMouse = mouse;
        isLaunchingFood = true;
    }

    public void ThrowFood(Vector2 direction)
    {
        SetEquation2Throw(direction);
        foodItem.transform.parent = null;
        foodItem = null;
        targetMouse = null;
        isLaunchingFood = false;
        length = 1f;
    }

    private void SetEquation2Throw(Vector2 direction)
    {
        Vector3 mousePos = direction * length;

        var strength = mousePos * playerForce;

        float velocity = math.sqrt(math.pow(strength.x, 2) + math.pow(strength.y, 2));
        float distance = math.sqrt(math.pow(mousePos.x, 2) + math.pow(mousePos.y, 2));
        
        float acceleration = (math.pow(velocity, 2)) / (2 * distance);
        Vector2 negativeAcceleration = (-acceleration * mousePos / distance);

        foodItem.Throw(strength, negativeAcceleration, velocity / acceleration);
    }
}
