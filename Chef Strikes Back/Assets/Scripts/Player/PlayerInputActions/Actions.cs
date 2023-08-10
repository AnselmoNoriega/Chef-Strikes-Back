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

    [Space, Header("Player Attack")]
    [SerializeField] CharacterMovement CM;
    [SerializeField] Player player;


    private void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        item.Add(collision.GetComponent<Item>());
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
        if (item.Count > 0)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(mouse.ReadValue<Vector2>());

            for (int i = 0; i < item.Count; i++)
            {
                if (item[i].collider.OverlapPoint(mousePos))
                {
                    inventory.AddItem(item[i]);
                    return;
                }
            }
        }
    }

    public void ThrowItem(InputAction mouse)
    {
        if (inventory.GetFoodItem() != null)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(mouse.ReadValue<Vector2>());
            var strength = (mousePos - transform.position) * throwForce;

            float velocity = math.sqrt(math.pow(strength.x, 2) + math.pow(strength.y, 2));
            float distance = math.sqrt((mousePos.x - transform.position.x) * (mousePos.x - transform.position.x) +
                                        (mousePos.y - transform.position.y) * (mousePos.y - transform.position.y));

            float acceleration = (math.pow(velocity, 2)) / (2 * distance);
            Vector2 negativeAcceleration = new Vector2(-acceleration * (mousePos.x - transform.position.x) / distance,
                                                       -acceleration * (mousePos.y - transform.position.y) / distance);

            inventory.ThrowFood(strength, negativeAcceleration, velocity / acceleration);
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
