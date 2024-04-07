using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Actions : MonoBehaviour
{
    private Player _player;

    [Header("Inventory Actions")]
    [SerializeField] private float throwForce;
    private bool _ready2Throw;
    private Inventory _inventory;
    private bool _isCarryingItem;

    [Space, Header("Player Grab")]
    [SerializeField] private float _grabDistance;
    private SpriteRenderer _selectedItem = null;

    private void Start()
    {
        _player = GetComponent<Player>();
        _inventory = GetComponent<Inventory>();
        _ready2Throw = false;
        _isCarryingItem = false;
    }

    public void Check4CloseItems(InputAction mouse)
    {
        if (!_isCarryingItem)
        {
            Collider2D[] hits;

            if (mouse == null)
            {
                var center = (Vector2)_player.transform.position + (_player.LookingDirection / 3);
                hits = Physics2D.OverlapCircleAll(center, 0.4f, 1);
            }
            else
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(mouse.ReadValue<Vector2>());
                hits = Physics2D.OverlapCircleAll(mousePos, 0.01f, 1);
            }

            Check4ItemRayCast(hits);
        }
        else if (_selectedItem)
        {
            _selectedItem.enabled = false;
            _selectedItem = null;
        }
    }

    private void Check4ItemRayCast(Collider2D[] hits)
    {
        float distance = 1.0f;
        SpriteRenderer newItem = null;
        float tempDis;

        foreach (var hit in hits)
        {
            float dis2Obj = Vector2.Distance(hit.gameObject.transform.position, transform.position);
            if (dis2Obj > _grabDistance)
            {
                continue;
            }

            tempDis = math.abs(hit.transform.position.magnitude - transform.position.magnitude);

            if (tempDis < distance && hit.GetComponent<Item>() && hit.GetComponent<Item>().IsPickable)
            {
                distance = tempDis;
                newItem = hit.GetComponent<Item>().GetHighlight();
                break;
            }
            else if (tempDis < distance && hit.GetComponent<FoodPile>())
            {
                distance = tempDis;
                newItem = hit.GetComponent<FoodPile>().GetOutline();
                break;
            }
        }

        if (newItem && _selectedItem != newItem)
        {
            if (_selectedItem)
            {
                _selectedItem.enabled = false;
            }
            newItem.enabled = true;
            _selectedItem = newItem;
        }
        else if (!newItem && _selectedItem)
        {
            _selectedItem.enabled = false;
            _selectedItem = null;
        }
    }

    public void GrabItem()
    {
        if (!_isCarryingItem && _selectedItem)
        {
            var parent = _selectedItem.transform.parent;

            Item item = parent.GetComponent<Item>();
            if (item)
            {
                _inventory.AddItem(item);
                _isCarryingItem = true;
                item.CollidersState(false);
                return;
            }

            FoodPile pile = parent.GetComponent<FoodPile>();
            if (pile)
            {
                var newFoodPileItem = pile.Hit();
                _inventory.AddItem(newFoodPileItem.GetComponent<Item>());
                _isCarryingItem = true;
                ServiceLocator.Get<AudioManager>().PlaySource("food_hit");
                newFoodPileItem.GetComponent<Item>().CollidersState(false);
                return;
            }
        }
    }

    public void PrepareToThrow(InputAction mouse)
    {
        if (_inventory.GetFoodItem() != null)
        {
            _inventory.PrepareToThrowFood(mouse);
            _ready2Throw = true;
            _player.ChangeAction(PlayerActions.Throwing);
        }
    }

    public void ThrowItem()
    {
        if (_inventory.GetFoodItem() != null && _ready2Throw)
        {
            ServiceLocator.Get<AudioManager>().PlaySource("charge");
            _inventory.ThrowFood(_player.Variables.ThrowDirection);
            _ready2Throw = false;
            _isCarryingItem = false;
            _player.ChangeAction(PlayerActions.None);
        }
    }

    public void DropItem()
    {
        if (_inventory.GetFoodItem() != null)
        {
            _inventory.ThrowFood(Vector2.zero);
        }
        _ready2Throw = false;
        _isCarryingItem = false;
        _player.ChangeAction(PlayerActions.None);
    }

    public void Attacking(Vector2 anglePos)
    {
        if (_player.PlayerAction != PlayerActions.Attacking && !_ready2Throw)
        {
            if (anglePos != Vector2.zero)
            {
                Vector3 rayOrigin = new Vector2(_player.transform.position.x, _player.transform.position.y + 0.35f);
                _player.LookingDirection = (Camera.main.ScreenToWorldPoint(anglePos) - rayOrigin).normalized;
            }
            _player.ChangeAction(PlayerActions.Attacking);
        }
    }
}
