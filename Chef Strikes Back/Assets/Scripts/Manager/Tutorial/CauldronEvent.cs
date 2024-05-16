using UnityEngine;

public class CauldronEvent : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Food")
        {
            var component = collision.gameObject.GetComponent<Item>();
            var tutorialManager = ServiceLocator.Get<TutorialLoopManager>();

            if (component && tutorialManager.TutorialSecondFace)
            {
                tutorialManager.TriggerSpaghettiEvent(component.Type == FoodType.Spaghetti);
            }
            else if (component)
            {
                tutorialManager.TriggerCauldronEvent(component.Type == FoodType.Pizza);
            }
        }
    }
}
