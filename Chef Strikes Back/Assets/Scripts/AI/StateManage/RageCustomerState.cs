using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageCustomerState : StateClass<AI>
{
    public void Enter(AI agent)
    {
        GameManager.Instance.AIPool.Add(agent.gameObject);
    }

    public void Update(AI agent, float dt)
    {
        if (agent.aiData.currentTarget != null)
        {
            agent.OnPointerInput?.Invoke(agent.aiData.currentTarget.position);

            if (agent.chasing == false)
            {
                agent.chasing = true;
                agent.StartCoroutine(agent.ChaseAndAttack());
            }
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