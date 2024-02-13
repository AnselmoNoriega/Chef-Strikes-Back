using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Actions : MonoBehaviour
{
    [Header("Inventory Actions")]
    private Inventory inventory;
    [SerializeField] private float throwForce;
    public bool ready2Throw;
    private bool isCarryingItem;

    [Space, Header("Player Throw")]
    private Vector3 offset;

    [Space, Header("Player Attack")]
    [SerializeField] private Player player;
    public float PlayerAttackRange;

    [Space, Header("Player Grab")]
    [SerializeField] private float grabDistance;
    private Item _selectedItem = null;


    private void Start()
    {
        inventory = GetComponent<Inventory>();
        ready2Throw = false;
        isCarryingItem = false;
        offset = new Vector3(0, 0.35f, 0);
    }

    public void Check4CloseItems()
    {
        if (!isCarryingItem)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)player.transform.position + (player.LookingDirection / 3), 0.4f);
            float distance = 1000;
            Item newItem = null;
            float tempDis;

            foreach (var hit in hits)
            {
                tempDis = math.abs(hit.transform.position.magnitude - transform.position.magnitude);

                if (tempDis < distance && hit.GetComponent<Item>() && hit.GetComponent<Item>().IsPickable)
                {
                    distance = tempDis;
                    newItem = hit.GetComponent<Item>();
                }
            }

            if (newItem != null && _selectedItem != newItem)
            {
                if (_selectedItem != null)
                {
                    _selectedItem.ActivateLight(false);
                    _selectedItem = null;
                }
                newItem.ActivateLight(true);
                _selectedItem = newItem;
            }
            else if (newItem == null && _selectedItem != null)
            {
                _selectedItem.ActivateLight(false);
                _selectedItem = null;
            }
        }
        else if (_selectedItem != null)
        {
            _selectedItem.ActivateLight(false);
            _selectedItem = null;
        }
    }

    public void GrabItem(InputAction mouse)
    {
        if (!isCarryingItem)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(mouse.ReadValue<Vector2>());

            Vector3 pos = new Vector2(player.transform.position.x, player.transform.position.y + 0.35f);
            player.LookingDirection = (Camera.main.ScreenToWorldPoint(mouse.ReadValue<Vector2>()) - pos).normalized;
            PlayerHelper.FaceMovementDirection(player.Animator, player.LookingDirection);

            Collider2D[] hits = Physics2D.OverlapCircleAll(mousePos, 0.01f);

            FoodPile foodPile = null;

            foreach (var hit in hits)
            {
                var myItem = hit.GetComponent<Item>();
                if (myItem && myItem.IsPickable && Vector2.Distance(transform.position, myItem.gameObject.transform.position) < grabDistance)
                {
                    inventory.AddItem(myItem);
                    isCarryingItem = true;
                    myItem.CollidersState(false);
                    return;
                }

                if (hit.GetComponent<FoodPile>())
                {
                    foodPile = hit.GetComponent<FoodPile>();
                }
            }

            if (foodPile != null && Vector2.Distance(foodPile.transform.position, transform.position) < grabDistance)
            {
                var newItem = foodPile.Hit();
                inventory.AddItem(newItem.GetComponent<Item>());
                isCarryingItem = true;
                ServiceLocator.Get<AudioManager>().PlaySource("food_hit");
                newItem.GetComponent<Item>().CollidersState(false);
                return;
            }
        }
    }

    public void GrabItem()
    {
        if (!isCarryingItem)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)player.transform.position + (player.LookingDirection / 3), 0.4f);
            float distance = 1000;
            Item newItem = null;
            FoodPile foodPile = null;
            float tempDis;

            foreach (var hit in hits)
            {
                tempDis = math.abs(hit.transform.position.magnitude - transform.position.magnitude);

                if (tempDis < distance && hit.GetComponent<Item>() && hit.GetComponent<Item>().IsPickable)
                {
                    distance = tempDis;
                    newItem = hit.GetComponent<Item>();
                }
                else if (hit.GetComponent<FoodPile>())
                {
                    foodPile = hit.GetComponent<FoodPile>();
                }
            }

            if (newItem != null)
            {
                inventory.AddItem(newItem);
                isCarryingItem = true;
                newItem.CollidersState(false);
            }
            else if (foodPile != null)
            {
                var newFoodPileItem = foodPile.Hit();
                inventory.AddItem(newFoodPileItem.GetComponent<Item>());
                isCarryingItem = true;
                ServiceLocator.Get<AudioManager>().PlaySource("food_hit");
                newFoodPileItem.GetComponent<Item>().CollidersState(false);
            }
        }
    }

    public void PrepareToThrow(InputAction mouse)
    {
        if (inventory.GetFoodItem() != null)
        {
            player.Mouse = mouse;
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
                ServiceLocator.Get<AudioManager>().PlaySource("charge");
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
        if (player.PlayerAction != PlayerActions.Attacking && !ready2Throw)
        {
            if (anglePos != Vector2.zero)
            {
                Vector3 rayOrigin = new Vector2(player.transform.position.x, player.transform.position.y + 0.35f);
                player.LookingDirection = (Camera.main.ScreenToWorldPoint(anglePos) - rayOrigin).normalized;
            }
            player.ChangeAction(PlayerActions.Attacking);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector2)player.transform.position + (player.LookingDirection / 3), PlayerAttackRange);
        Gizmos.color = Color.blue;
    }

}
