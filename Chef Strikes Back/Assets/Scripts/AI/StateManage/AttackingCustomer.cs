using UnityEngine;
using Pathfinding;

public class AttackingCustomer : StateClass<AI>
{
    private float _countDown = 0;
    private Vector2 _playerPos = Vector2.zero;

    public void Enter(AI agent)
    {
        _playerPos = ServiceLocator.Get<Player>().transform.position;
        _countDown = Time.time;
        Attack();
    }

    public void Update(AI agent, float dt)
    {
        if (Time.time - _countDown >= 0.75f)
        {
            agent.ChangeState(AIState.Rage);
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

    private void Attack()
    {

    }
}
