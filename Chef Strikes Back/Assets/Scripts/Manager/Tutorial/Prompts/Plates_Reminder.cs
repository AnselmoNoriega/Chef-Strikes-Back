using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plates_Reminder : MonoBehaviour
{
    [SerializeField] public GameObject plates_Reminders;
    private bool _isActive = true;
    private void Update()
    {

        if (IsHoldingFood() && _isActive)
        {
            _isActive = false;
            ButtonFlashing(plates_Reminders);
        }
        else if(!IsHoldingFood()&&!_isActive)
        {
            _isActive=true;
            plates_Reminders.SetActive(false);
        }

    }
    private IEnumerator ButtonFlashing(GameObject gameObject)
    {
        while (IsSitting() && IsHoldingFood())
        {
            gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
    }
    private bool IsHoldingFood()
    {
        var inventory = ServiceLocator.Get<Player>().GetComponent<Inventory>();
        if (inventory.CarryItemType() == FoodType.Pizza && inventory.CarryItemType() == FoodType.Spaghetti)
        {
            return true;
        }
        return false;
    }
    public bool IsSitting()
    {
        var chair = ServiceLocator.Get<Chair>();
        return chair.seatAvaliable;
    }
}
