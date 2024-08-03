using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CB_Reminder : MonoBehaviour
{
    private enum ReminderIndex
    {
        Pizza,
        Spaghetti,
        None
    }

    [SerializeField] private List<GameObject> cb_Reminder;

    private ReminderIndex _reminderIndex = ReminderIndex.Pizza;
    private bool _shouldBeActive = false;

    private IEnumerator ButtonFlashing(GameObject gameObject)
    {
        while (_shouldBeActive)
        {
            gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void StartArrowEvent(FoodType foodType)
    {
        _reminderIndex = (ReminderIndex)foodType;
        _shouldBeActive = true;
        StartCoroutine(ButtonFlashing(cb_Reminder[(int)foodType]));
    }

    public void FoodCreated(FoodType foodType)
    {
        if (_reminderIndex == (ReminderIndex)foodType)
        {
            _shouldBeActive = false;
        }
    }
}
