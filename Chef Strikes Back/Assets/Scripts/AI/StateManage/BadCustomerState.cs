using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadCustomerState : StateClass<AI>
{
    bool isStand;

    public void Enter(AI agent)
    {
        agent.GetComponent<SpriteRenderer>().color = Color.red;
        PathRequestManager.RequestPath(agent.transform.position, TileManager.Instance.requestEmptyPos(), agent.OnPathFound);
        isStand = true;
        Debug.Log("BadCustomer");
    }

    public void Update(AI agent, float dt)
    {
        if (!isStand)
        {
            PathRequestManager.RequestPath(agent.transform.position, TileManager.Instance.requestEmptyPos(), agent.OnPathFound);
            isStand = true;
        }

        if (agent.isHit)
        {
            isStand = false;
            agent.isHit = false;
        }

        if (GameManager.Instance.rageMode)
        {
            PathRequestManager.RequestPath(agent.transform.position, agent.transform.position, agent.OnPathFound);
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
            isStand = false;
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(-rb.velocity * 100, ForceMode2D.Impulse);
            var rage = collision.gameObject.GetComponent<Player>();
            if (rage)
            {
                GameManager.Instance.RageValue += 10;
                rage.currentRage += 10;
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
