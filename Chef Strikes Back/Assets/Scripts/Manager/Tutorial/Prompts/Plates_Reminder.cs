using System.Collections;
using UnityEngine;

public class Plates_Reminder : MonoBehaviour
{
    [SerializeField] public GameObject plates_Reminders;
    private bool _isActive = false;
    private void Awake()
    {
        plates_Reminders.SetActive(false);
    }

    private void Update()
    {
        if (IsHoldingFood() && !_isActive)
        {
            _isActive = true;
            StartCoroutine(ButtonFlashing(plates_Reminders));
        }
        else if (!IsHoldingFood() && _isActive)
        {
            _isActive = false;
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
        if (inventory.CarryItemType() == FoodType.Pizza || inventory.CarryItemType() == FoodType.Spaghetti)
        {
            return true;
        }
        return false;
    }
    public bool IsSitting()
    {
        var chair = GetComponent<Chair>();
        return !chair.seatAvaliable;
    }
}
