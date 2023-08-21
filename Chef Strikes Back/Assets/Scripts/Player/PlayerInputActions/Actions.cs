using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Actions : MonoBehaviour
{
    [Header("Inventory Actions")]
    public List<Item> item;
    private Inventory inventory;
    [SerializeField]
    private float throwForce;
    private bool ready2Throw;
    private bool isCarryingItem;

    [Space, Header("Player Attack")]
    [SerializeField] CharacterMovement CM;
    [SerializeField] Player player;


    private void Start()
    {
        inventory = GetComponent<Inventory>();
        ready2Throw = false;
        isCarryingItem = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            item.Add(collision.GetComponent<Item>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            item.Remove(collision.GetComponent<Item>());
        }
    }

    public void GrabItem(InputAction mouse)
    {
        if (item.Count > 0 && !isCarryingItem)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(mouse.ReadValue<Vector2>());

            for (int i = 0; i < item.Count; i++)
            {
                if (item[i] && item[i].collider.OverlapPoint(mousePos))
                {
                    inventory.AddItem(item[i]);
                    isCarryingItem = true;
                    return;
                }
            }
        }
    }

    public void PrepareToThrow(InputAction mouse)
    {
        if (inventory.GetFoodItem() != null)
        {
            inventory.PrepareToThrowFood(mouse);
            ready2Throw = true;
        }
    }

    public void ThrowItem(InputAction mouse)
    {
        if (inventory.GetFoodItem() != null && ready2Throw)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(mouse.ReadValue<Vector2>());
            var dir = (mousePos - transform.position);
            dir.z = 0;
            dir.Normalize();
            inventory.ThrowFood(dir);
            ready2Throw = false;
            isCarryingItem = false;
        }
    }

    public void Attacking(InputAction mouse)
    {
        var mousePos = Camera.main.ScreenToWorldPoint(mouse.ReadValue<Vector2>());

        player.Attack(mousePos);
    }

    public void Boosting()
    {
        CM.SpeedBoost(5.0f);
    }

    public void BoostReleased()
    {
        CM.SpeedBoost(-5.0f);
    }

}
