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
        agent.isSit = false;
    }

    public void Update(AI agent, float dt)
    {
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
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(-rb.velocity.normalized * 60, ForceMode2D.Impulse);
            var rage = collision.gameObject.GetComponent<Player>();
            if (rage)
            {
                rage.TakeRage(10);
                ServiceLocator.Get<AudioManager>().PlaySource("ragebar_filling");
            }
        }
    }

    public void TriggerEnter2D(AI agent, Collider2D collision)
    {

    }

    public void Exit(AI agent)
    {

    }
}
