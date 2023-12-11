using UnityEngine;

public class BadCustomerState : StateClass<AI>
{
    public void Enter(AI agent)
    {
        agent._gameLoopManager.AddBadAI(agent.gameObject);
        ServiceLocator.Get<GameManager>().EnterRageModeScore();
        ServiceLocator.Get<Player>().TakeRage(10);
        agent.GetComponent<SpriteRenderer>().color = Color.red;
        agent.gameObject.GetComponent<Rigidbody2D>().constraints &= RigidbodyConstraints2D.FreezeRotation;
        agent.aiData.Target = null;
        agent.aiData.isStand = false;
        agent.isSit = false;
    }

    public void Update(AI agent, float dt)
    {
        if (agent.aiData.currentTarget != null)
        {
            agent.OnPointerInput?.Invoke(agent.aiData.currentTarget.position);
            agent.FindStandPoint();
        }
        else if (agent.aiData.GetTargetsCount() > 0)
        {
            agent.aiData.currentTarget = agent.aiData.targets[0];
        }

        agent.OnMovementInput?.Invoke(agent.movementInput);

        if (agent.isHit)
        {
            agent.aiData.isStand = false;
            agent.isHit = false;
        }

        if(agent._gameLoopManager.IsInRageMode()) 
        {
            agent.ChangeState(AIState.Rage);
        }
    }

    public void FixedUpdate(AI agent)
    {

    }

    public void CollisionEnter2D(AI agent, Collision2D collision)
    {
        if (collision.transform.tag == "Player" || collision.transform.tag == "Food")
        {
            agent.aiData.isStand = false;
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(-rb.velocity * 100, ForceMode2D.Impulse);
            var rage = collision.gameObject.GetComponent<Player>();
            if (rage)
            {
                rage.TakeRage(10);
                ServiceLocator.Get<AudioManager>().PlaySource("ragebar_filling");
            }
        }
        if(collision.transform.tag == "Enemy" || collision.transform.tag == "Obstacle" || collision.transform.tag == "Chair")
        {
            agent.aiData.isStand = false;
        }
    }

    public void TriggerEnter2D(AI agent, Collider2D collision)
    {

    }

    public void Exit(AI agent)
    {
        agent.aiData.currentTarget = null;
        agent.aiData.targets = null;
    }
}
