using UnityEngine;

public class GoodCustomerState : StateClass<AI>
{
    public void Enter(AI agent)
    {

    }

    public void Update(AI agent, float dt)
    {
        

        if (!agent.eating && agent.isSit && !agent._gameLoopManager.IsInRageMode())
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




