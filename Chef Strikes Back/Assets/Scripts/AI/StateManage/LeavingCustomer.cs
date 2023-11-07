using UnityEngine;

public class LeavingCustomer : StateClass<AI>
{
    Vector3 ExitPos;

    public void Enter(AI agent)
    {
        agent.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        ExitPos = ServiceLocator.Get<TileManager>().requestEntrancePos();
        PathRequestManager.RequestPath(agent.transform.position, ExitPos, agent.OnPathFound);
    }

    public void Update(AI agent, float dt)
    {
        if (agent.transform.position == ExitPos)
        {
            agent.isExist = true;
        }

        if (agent.aiData.currentTarget != null)
        {
            agent.OnPointerInput?.Invoke(agent.aiData.currentTarget.position);
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
