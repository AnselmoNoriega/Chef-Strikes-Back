using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CB_Reminder : MonoBehaviour
{
    [SerializeField] private List<GameObject> cb_Reminder;
    private bool _isActive = false;

    void Update()
    {
        if (IsHolding() && !_isActive)
        {
            _isActive = true;
            foreach (var reminder in cb_Reminder)
            {
                StartCoroutine(ButtonFlashing(reminder));
            }
        }
        else if (!IsHolding() && _isActive)
        {
            _isActive = false;
            foreach (var reminder in cb_Reminder)
            {
                reminder.SetActive(false);
            }
        }
    }

    private bool IsHolding()
    {
        if (IsHoldingFood())
        {
            var actions = ServiceLocator.Get<Player>().GetComponent<Actions>();
            return actions.IsCarryingItem;
        }
        return false;
    }
    private bool IsHoldingFood()
    {
        var inventory = ServiceLocator.Get<Player>().GetComponent<Inventory>();
        if (inventory.CarryItemType() != FoodType.Pizza && inventory.CarryItemType() != FoodType.Spaghetti && inventory.CarryItemType() != FoodType.None)
        {
            return true;
        }
        return false;
    }
    private IEnumerator ButtonFlashing(GameObject gameObject)
    {
        while (_isActive)
        {
            gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
