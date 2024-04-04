using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Actions : MonoBehaviour
{
    [Header("Inventory Actions")]
    [SerializeField] private float _throwForce;
    private bool _ready2Throw;
    private Inventory _inventory;
    private bool _isCarryingItem;

    [Space, Header("Player Throw")]
    private Vector3 _throwOffset;

    [Space, Header("Player Attack")]
    [SerializeField] private Player _player;
    private float _playerAttackRange;

    [Space, Header("Player Grab")]
    [SerializeField] private float _grabDistance;
    private SpriteRenderer _selectedItem = null;

    private void Start()
    {
        _inventory = GetComponent<Inventory>();
        _ready2Throw = false;
        _isCarryingItem = false;
        _throwOffset = new Vector3(0, 0.35f, 0);
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
            if(dis2Obj > _grabDistance)
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

    public void GrabItem(InputAction mouse)
    {
        if (_isCarryingItem)
        {
            return;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(mouse.ReadValue<Vector2>());

        Vector3 pos = new Vector2(_player.transform.position.x, _player.transform.position.y + 0.35f);
        _player.LookingDirection = (Camera.main.ScreenToWorldPoint(mouse.ReadValue<Vector2>()) - pos).normalized;
        PlayerHelper.FaceMovementDirection(_player.Variables.PlayerAnimator, _player.LookingDirection);

        Collider2D[] hits = Physics2D.OverlapCircleAll(mousePos, 0.01f);

        FoodPile foodPile = null;
        Item lastItem = null;

        foreach (var hit in hits)
        {
            var myItem = hit.GetComponent<Item>();
            if (myItem && myItem.IsPickable && Vector2.Distance(transform.position, myItem.gameObject.transform.position) < _grabDistance)
            {
                if ((int)myItem.Type <= 1)
                {
                    _inventory.AddItem(myItem);
                    _isCarryingItem = true;
                    myItem.CollidersState(false);
                    return;
                }
                else
                {
                    lastItem = myItem;
                    continue;
                }
            }

            if (hit.GetComponent<FoodPile>())
            {
                foodPile = hit.GetComponent<FoodPile>();
            }
        }

        if (lastItem)
        {
            _inventory.AddItem(lastItem);
            _isCarryingItem = true;
            lastItem.CollidersState(false);
            return;
        }

        if (foodPile && Vector2.Distance(foodPile.transform.position, transform.position) < _grabDistance)
        {
            var newItem = foodPile.Hit();
            _inventory.AddItem(newItem.GetComponent<Item>());
            _isCarryingItem = true;
            ServiceLocator.Get<AudioManager>().PlaySource("food_hit");
            newItem.GetComponent<Item>().CollidersState(false);
            return;
        }
    }

    public void GrabItem()
    {
        if (!_isCarryingItem)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)_player.transform.position + (_player.LookingDirection / 3), 0.4f);
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
                _inventory.AddItem(newItem);
                _isCarryingItem = true;
                newItem.CollidersState(false);
            }
            else if (foodPile != null)
            {
                var newFoodPileItem = foodPile.Hit();
                _inventory.AddItem(newFoodPileItem.GetComponent<Item>());
                _isCarryingItem = true;
                ServiceLocator.Get<AudioManager>().PlaySource("food_hit");
                newFoodPileItem.GetComponent<Item>().CollidersState(false);
            }
        }
    }

    public void PrepareToThrow(InputAction mouse)
    {
        if (_inventory.GetFoodItem() != null)
        {
            _player.Mouse = mouse;
            _inventory.PrepareToThrowFood(mouse);
            _ready2Throw = true;
            _player.ChangeAction(PlayerActions.Throwing);
        }
    }

    public void ThrowItem(InputAction pos)
    {
        if (_inventory.GetFoodItem() != null && _ready2Throw)
        {
            var dir = pos.ReadValue<Vector2>();

            if (dir.magnitude >= 10.0f)
            {
                var mousePos = Camera.main.ScreenToWorldPoint(pos.ReadValue<Vector2>());
                dir = (mousePos - (transform.position + _throwOffset));
                dir.Normalize();
                ServiceLocator.Get<AudioManager>().PlaySource("charge");
            }

            _inventory.ThrowFood(dir);
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector2)_player.transform.position + (_player.LookingDirection / 3), _playerAttackRange);
        Gizmos.color = Color.blue;
    }

}
