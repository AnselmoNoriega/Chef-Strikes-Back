using UnityEngine;
using Pathfinding;

public class AttackingCustomer : StateClass<AI>
{
    private float _countDown = 0;
    private bool _hasAttacked = false;

    public void Enter(AI agent)
    {
        _hasAttacked = false;
        _countDown = Time.time;
    }

    public void Update(AI agent, float dt)
    {
        if (Time.time - _countDown >= 0.25f && !_hasAttacked)
        {
            _hasAttacked = true;
            var player = ServiceLocator.Get<Player>();
            Vector2 dirToCollider = (player.transform.position - agent.transform.position).normalized;
            player.Rb.AddForce(dirToCollider * agent.knockbackForce, ForceMode2D.Impulse);
            player.TakeDamage(10);
        }

        if (Time.time - _countDown >= 1.0f)
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
}
