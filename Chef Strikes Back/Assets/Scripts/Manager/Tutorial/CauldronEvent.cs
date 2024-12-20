using UnityEngine;

public class CauldronEvent : MonoBehaviour
{
    [SerializeField] private CreationTable _creationTable;

    private TutorialLoopManager _loopManager;

    [SerializeField] private CB_Reminder _cbReminder;

    private int _creationTimes = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Food")
        {
            var component = collision.gameObject.GetComponent<Item>();
            _loopManager = ServiceLocator.Get<TutorialLoopManager>();

            if (component && _loopManager.TutorialSecondFace)
            {
                //_loopManager.TriggerSpaghettiEvent(component.Type == FoodType.Spaghetti);
                CheckCreationTimes();
                _cbReminder.FoodCreated(component.Type);
            }
            else if (component)
            {
                _loopManager.TriggerCauldronEvent(component.Type == FoodType.Pizza);
                CheckCreationTimes();
                _cbReminder.FoodCreated(component.Type);
            }
        }

        if (_creationTable.GetIsLocked() && collision.transform.tag == "Ingredient")
        {
            _loopManager.EnterDialogueEvent("TableLocked");
        }
    }

    private void CheckCreationTimes()
    {
        ++_creationTimes;
        if (_creationTimes == 4)
        {
            _loopManager.EnterDialogueEvent("AfterThreeFood");
        }
    }
}
