using UnityEngine;

public class RageCustomerState : StateClass<AI>
{
    public void Enter(AI agent)
    {
        agent.gameObject.GetComponent<Rigidbody2D>().constraints &= RigidbodyConstraints2D.FreezeRotation;
       
    }

    public void Update(AI agent, float dt)
    {

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