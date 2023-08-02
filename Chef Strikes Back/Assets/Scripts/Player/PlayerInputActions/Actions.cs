using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Actions : MonoBehaviour
{
    [Header("Inventory Actions")]
    private Item item;
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
        item = collision.GetComponent<Item>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            item = null;
        }
    }

    public void GrabItem(InputAction mouse)
    {
        if (item != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(mouse.ReadValue<Vector2>());

            if (item.collider.OverlapPoint(mousePos))
            {
                inventory.AddItem(item);
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
            Debug.Log(negativeAcceleration);

            inventory.ThrowFood(strength, negativeAcceleration, velocity/acceleration);
        }
    }

    public void Attacking()
    {
        Vector2 attackDirection = CM.GetMoveDirection();
        player.SetAttackDirection(attackDirection);
        player.Attack();
    }

    public void Boosting()
    {
        CM.SpeedBoost(5.0f);
    }

    public void BoostReleased()
    {
        CM.SpeedBoost(-5.0f);
    }

    public void SwitchWeapon()
    {
        player.currentWeaponIndex = (player.currentWeaponIndex + 1) % player.weapons.Length;
        player.SwitchWeapon(player.currentWeaponIndex);
    }


}
