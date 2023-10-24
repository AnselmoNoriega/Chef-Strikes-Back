using UnityEngine;
using UnityEngine.InputSystem.Android;
public class CustomerLeavingState : StateClass<AI>
{
    public void Enter(AI agent)
    {
        Debug.Log("Leaving");
        agent.DropMoney();
        agent.OrderBubble.gameObject.SetActive(false);
    }

    public void Update(AI agent, float dt)
    {
        if (agent.aiData.currentTarget != null)
        {
            agent.OnPointerInput?.Invoke(agent.aiData.currentTarget.position);
            agent.FindSeat();

        }
        else if (agent.aiData.GetTargetsCount() > 0)
        {
            agent.aiData.currentTarget = agent.aiData.targets[0];
        }

        agent.OnMovementInput?.Invoke(agent.movementInput);
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

    }
}

