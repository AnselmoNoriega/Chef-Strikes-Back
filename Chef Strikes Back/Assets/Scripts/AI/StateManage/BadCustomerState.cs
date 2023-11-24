using UnityEngine;

public class BadCustomerState : StateClass<AI>
{
    public void Enter(AI agent)
    {
        ServiceLocator.Get<GameManager>().Score -= 2;
        ServiceLocator.Get<Player>().currentRage += 10;
        agent.GetComponent<SpriteRenderer>().color = Color.red;
        agent.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        agent.isStand = true;
    }

    public void Update(AI agent, float dt)
    {
        if (!agent.isStand && !agent._gameLoopManager.rageMode || agent.isLeaving)
        {
            PathRequestManager.RequestPath(agent.transform.position, ServiceLocator.Get<TileManager>().requestEmptyPos(), agent.OnPathFound);
            agent.isStand = true;
            agent.isLeaving = false;
        }
        agent.OnMovementInput?.Invoke(agent.movementInput);

        if (agent.isHit)
        {
            agent.isStand = false;
            agent.isHit = false;
        }

        if(agent._gameLoopManager.rageMode) 
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
            agent.isStand = false;
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(-rb.velocity * 100, ForceMode2D.Impulse);
            var rage = collision.gameObject.GetComponent<Player>();
            if (rage)
            {
                rage.currentRage += 10;
                ServiceLocator.Get<AudioManager>().PlaySource("ragebar_filling");

            }
        }
        if(collision.transform.tag == "Enemy" || collision.transform.tag == "Obstacle" || collision.transform.tag == "Chair")
        {
            agent.isStand = false;
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
