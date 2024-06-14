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
        Debug.Log("Update called");

        if (Time.time - _countDown >= 0.25f && !_hasAttacked)
        {
            Debug.Log("Time condition met and hasn't attacked yet");

            if (agent.IsDead)  // Check if the AI is dead
            {
                Debug.Log("Agent is dead, exiting early");
                return;  // Exit early if dead, preventing attack
            }

            _hasAttacked = true;
            Debug.Log("Agent attacking");

            var player = ServiceLocator.Get<Player>();
            Vector2 dirToCollider = (player.transform.position - agent.transform.position).normalized;
            player.Rb.AddForce(dirToCollider * agent.KnockbackForce, ForceMode2D.Impulse);
            player.TakeDamage(10);

            // Play random attack sound
            agent.PlayAttackSound();
        }

        if (Time.time - _countDown >= 1.0f)
        {
            Debug.Log("Changing state to Rage");
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
