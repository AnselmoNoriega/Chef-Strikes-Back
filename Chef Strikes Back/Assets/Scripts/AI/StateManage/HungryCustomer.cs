using UnityEngine;

public class HungryCustomer : StateClass<AI>
{
    private float eatingTime;
    private float waitingTime;

    public void Enter(AI agent)
    {
        eatingTime = 2.0f;
        waitingTime = 15.0f;

        agent.OrderBubble.gameObject.SetActive(true);
    }

    public void Update(AI agent, float dt)
    {
        waitingTime -= Time.deltaTime;

        if (agent.eating)
        {
            agent.OrderBubble.gameObject.SetActive(false);
            eatingTime -= Time.deltaTime;

            if (eatingTime <= 0)
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
        agent.OrderBubble.gameObject.SetActive(false);
    }
}
