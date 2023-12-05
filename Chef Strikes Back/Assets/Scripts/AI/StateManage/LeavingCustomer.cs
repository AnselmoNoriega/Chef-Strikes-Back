using UnityEngine;
using UnityEngine.InputSystem.Android;

public class LeavingCustomer : StateClass<AI>
{
    Vector3 ExitPos;

    public void Enter(AI agent)
    {
        agent.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        agent.aiData.currentTarget = null;
        agent.aiData.Target = null;
    }

    public void Update(AI agent, float dt)
    {
        if (agent.aiData.currentTarget != null)
        {
            agent.OnPointerInput?.Invoke(agent.aiData.currentTarget.position);
            agent.ExitRestaurant();
        }
        else if (agent.aiData.GetTargetsCount() > 0)
        {
            agent.aiData.currentTarget = agent.aiData.targets[0];
        }
        agent.OnMovementInput?.Invoke(agent.movementInput);

        if(agent.aiData.currentTarget == null) { agent.isExist = true; }
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
