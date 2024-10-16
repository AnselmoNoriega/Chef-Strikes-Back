using UnityEngine;

public class EatingTutorialCus : StateClass<AI>
{
    private float eatingTime = 2.0f;
    private float timer;
    private Vector3 scale;

    public void Enter(AI agent)
    {
        if(!ServiceLocator.Get<TutorialLoopManager>().TutorialSecondFace)
        {
        ServiceLocator.Get<TutorialLoopManager>().EnterConversation();
        }
        scale = agent.EatingSlider.localScale;
        agent.enabled = true;
        scale.x = 1;
        agent.EatingSlider.localScale = scale;
        agent.EatingSlider.transform.parent.gameObject.SetActive(true);

        agent.EatingSlider.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
        timer = eatingTime;
    }

    public void Update(AI agent, float dt)
    {
        scale.x -= Time.deltaTime / timer;
        agent.EatingSlider.localScale = scale;

        if (scale.x <= 0)
        {
            ServiceLocator.Get<Player>().MakeETransfer();
            agent.ChangeState(AIState.Leaving);
            agent.PlayMoneyUIPopUp();
            agent.SelectedChair.FreeTableSpace();
        }
    }

    public void FixedUpdate(AI agent)
    {

    }

    public void CollisionEnter2D(AI agent, Collision2D collision)
    {

    }

    public void TriggerEnter2D(AI agent, Collider2D collision)
    {

    }

    public void Exit(AI agent)
    {
        agent.EatingSlider.transform.parent.gameObject.SetActive(false);
    }
}
