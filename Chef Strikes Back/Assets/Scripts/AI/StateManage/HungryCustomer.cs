using UnityEngine;

public class HungryCustomer : StateClass<AI>
{
    private float waitingTime = 25.0f;
    private float eatingTime = 2.0f;
    private float timer;
    private Vector3 scale;

    public void Enter(AI agent)
    {
        scale = agent.EatingSlider.localScale;
        scale.x = 1;
        agent.EatingSlider.localScale = scale;
        agent.EatingSlider.transform.parent.gameObject.SetActive(true);

        if (agent.IsEating)
        {
            agent.EatingSlider.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
            timer = eatingTime;
        }
        else
        {
            if (agent.ChoiceIndex == 0)
            {
                agent._indicator.SetIndicator(true, IndicatorImage.Pizza);
                agent.OrderBubble[0].gameObject.SetActive(true);
            }
            else
            {
                agent._indicator.SetIndicator(true, IndicatorImage.Spaguetti);
                agent.OrderBubble[1].gameObject.SetActive(true);
            }
            agent.EatingSlider.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
            timer = waitingTime;
        }
    }

    public void Update(AI agent, float dt)
    {
        if (!ServiceLocator.Get<GameLoopManager>().IsInRageMode())
        {
            scale.x -= Time.deltaTime / timer;
            agent.EatingSlider.localScale = scale;
        }

        if (scale.x <= 0 && agent.IsEating)
        {
            agent.DropMoney();
            agent.ChangeState(AIState.Leaving);
        }
        else if (scale.x <= 0)
        {
            agent.ChangeState(AIState.Bad);
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
        if (agent.ChoiceIndex == 0)
        {
            agent._indicator.SetIndicator(false, IndicatorImage.Pizza);
            agent.OrderBubble[0].gameObject.SetActive(false);
        }
        else
        {
            agent._indicator.SetIndicator(false, IndicatorImage.Spaguetti);
            agent.OrderBubble[1].gameObject.SetActive(false);
        }
        agent.EatingSlider.transform.parent.gameObject.SetActive(false);
    }
}
