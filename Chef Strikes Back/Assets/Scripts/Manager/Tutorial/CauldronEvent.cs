using UnityEngine;

public class CauldronEvent : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Food")
        {
            var component = collision.gameObject.GetComponent<Item>();
            if (component)
            {
                ServiceLocator.Get<TutorialLoopManager>().TriggerCauldronEvent(component.Type == FoodType.Pizza);
            }
        }
    }
}
