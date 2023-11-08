using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.InputSystem;

public class Actions : MonoBehaviour
{
    [Header("Inventory Actions")]
    public List<Item> item;
    private Inventory inventory;
    [SerializeField]
    private float throwForce;
    public bool ready2Throw;
    private bool isCarryingItem;

    [Space, Header("Player Throw")]
    private Vector3 offset;

    [Space, Header("Player Attack")]
    [SerializeField]
    private Player player;


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
        if (item.Count > 0 && !isCarryingItem && !ServiceLocator.Get<GameLoopManager>().rageMode)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(mouse.ReadValue<Vector2>());

            for (int i = 0; i < item.Count; i++)
            {
                if (item[i] && item[i].GetComponent<Collider2D>().OverlapPoint(mousePos) && item[i].isPickable)
                {
                    inventory.AddItem(item[i]);
                    isCarryingItem = true;
                    item[i].GetComponent<CircleCollider2D>().enabled = false;
                    return;
                }
            }
        }
    }

    public void GrabItem()
    {
        if (!isCarryingItem && !ServiceLocator.Get<GameLoopManager>().rageMode)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)player.transform.position + (player.lookingDirection / 3), 0.4f);
            float distance = 1000;
            Item newItem = null;
            float tempDis;

            foreach (var hit in hits)
            {
                tempDis = math.abs(hit.transform.position.magnitude - transform.position.magnitude);

                if (tempDis < distance && hit.GetComponent<Item>() && hit.GetComponent<Item>().isPickable)
                {
                    distance = tempDis;
                    newItem = hit.GetComponent<Item>();
                }
            }

            if (newItem != null)
            {
                inventory.AddItem(newItem);
                isCarryingItem = true;
                newItem.GetComponent<CircleCollider2D>().enabled = false;
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
            player.ChangeAction(PlayerActions.Throwing);
        }
    }

    public void ThrowItem(InputAction pos)
    {
        if (inventory.GetFoodItem() != null && ready2Throw)
        {
            var dir = pos.ReadValue<Vector2>();

            if (dir.magnitude >= 10.0f)
            {
                var mousePos = Camera.main.ScreenToWorldPoint(pos.ReadValue<Vector2>());
                dir = (mousePos - (transform.position + offset));
                dir.Normalize();
            }

            inventory.ThrowFood(dir);
            ready2Throw = false;
            isCarryingItem = false;
            player.ChangeAction(PlayerActions.None);
        }
    }

    public void DropItem()
    {
        if (inventory.GetFoodItem() != null)
        {
            inventory.ThrowFood(Vector2.zero);
        }
        ready2Throw = false;
        isCarryingItem = false;
        player.ChangeAction(PlayerActions.None);
    }

    public void Attacking(Vector2 anglePos)
    {
        if (player.playerAction != PlayerActions.Attacking && !ready2Throw)
        {
            if (anglePos != Vector2.zero)
            {
                Vector3 rayOrigin = new Vector2(player.transform.position.x, player.transform.position.y + 0.35f);
                player.lookingDirection = (Camera.main.ScreenToWorldPoint(anglePos) - rayOrigin).normalized;
            }
            player.ChangeAction(PlayerActions.Attacking);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector2)player.transform.position + (player.lookingDirection/3), 0.4f);
    }

}
