using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadCustomerState : StateClass<AI>
{

    public void Enter(AI agent)
    {
        agent.isAngry = true;
        agent.GetComponent<SpriteRenderer>().color = Color.red;
        PathRequestManager.RequestPath(agent.transform.position, TileManager.Instance.requestEmptyPos(), agent.OnPathFound);
        agent.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        agent.isStand = true;
    }

    public void Update(AI agent, float dt)
    {
        if (!agent.isStand && !GameManager.Instance.rageMode)
        {
            PathRequestManager.RequestPath(agent.transform.position, TileManager.Instance.requestEmptyPos(), agent.OnPathFound);
            agent.isStand = true;
        }

        if (agent.isHit)
        {
            agent.isStand = false;
            agent.isHit = false;
        }

        if(GameManager.Instance.rageMode) 
        {
            agent.stateManager.ChangeState((int)AIState.Rage);
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
            }
        }
        if(collision.transform.tag == "Enemy")
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
