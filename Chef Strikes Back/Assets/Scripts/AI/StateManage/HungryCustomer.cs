using UnityEngine;

public class HungryCustomer : StateClass<AI>
{
    private float waitingTime;
    private float eatingTime = 2.0f;
    private Vector3 scale;

    public void Enter(AI agent)
    {
        scale = agent.eatingSlider.localScale;
        scale.x = 1;
        agent.eatingSlider.localScale = scale;
        waitingTime = 15.0f;

        agent.OrderBubble.gameObject.SetActive(true);
    }

    public void Update(AI agent, float dt)
    {
        waitingTime -= Time.deltaTime;

        if (agent.eating)
        {
            scale.x -= Time.deltaTime / eatingTime;
            agent.eatingSlider.localScale = scale;

            if (scale.x <= 0)
            {
                agent.DropMoney();
                agent.ChangeState(AIState.Leaving);
            }
        }
        else if (waitingTime <= 0)
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
        agent.eatingSlider.transform.parent.gameObject.SetActive(true);
        agent.OrderBubble.gameObject.SetActive(false);
    }
}
