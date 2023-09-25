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

    [Space, Header("Player Throw")]
    private Vector3 offset;

    [Space, Header("Player Attack")]
    [SerializeField] CharacterMovement CM;
    [SerializeField] Player player;


    private void Start()
    {
        inventory = GetComponent<Inventory>();
        ready2Throw = false;
        isCarryingItem = false;
        offset = new Vector3(0, 0.35f, 0);
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
                if (item[i] && item[i].GetComponent<Collider2D>().OverlapPoint(mousePos))
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
            player.mouse = mouse;
            inventory.PrepareToThrowFood(mouse);
            ready2Throw = true;
            player.ChangeState(PlayerStates.Throwing);
        }
    }

    public void ThrowItem(InputAction mouse)
    {
        if (inventory.GetFoodItem() != null && ready2Throw)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(mouse.ReadValue<Vector2>());
            var dir = (mousePos - (transform.position + offset));
            dir.z = 0;
            dir.Normalize();
            inventory.ThrowFood(dir);
            ready2Throw = false;
            isCarryingItem = false;
            player.ChangeState(PlayerStates.Idle);
        }
    }

    public void Attacking(InputAction mouse)
    {
        if (player.playerState != PlayerStates.Attacking)
        {
            player.attackDir = Camera.main.ScreenToWorldPoint(mouse.ReadValue<Vector2>());
            player.ChangeState(PlayerStates.Attacking);
        }
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
