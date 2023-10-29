using UnityEngine;

public class GoodCustomerState : StateClass<AI>
{
    public void Enter(AI agent)
    {
        agent.aiData.TargetLayerMask = LayerMask.GetMask("Chair");
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

        if (!agent.eating && agent.isSit && !GameManager.Instance.rageMode)
        {
            agent.ChangeState(AIState.Hungry);
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

    }
}




