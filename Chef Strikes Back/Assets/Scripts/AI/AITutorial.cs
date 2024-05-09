using UnityEngine;

public class AITutorial : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            var ai = collision.GetComponent<AI>();
            ai.enabled = false;
            ServiceLocator.Get<TutorialLoopManager>().EnterConversation();
            Destroy(gameObject);
        }

    }
}
