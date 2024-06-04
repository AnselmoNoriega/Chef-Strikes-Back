using UnityEngine;
using Pathfinding;

public class AttackingTutorialCus : StateClass<AI>
{
    private float _countDown = 0;
    private bool _hasAttacked = false;
    private Player player;

    public void Enter(AI agent)
    {
        player = ServiceLocator.Get<Player>();
        _hasAttacked = false;
        _countDown = Time.time;
    }

    public void Update(AI agent, float dt)
    {
        if (player.GotDamage)
        {
            ServiceLocator.Get<TutorialLoopManager>().EnterDialogueEvent("KillingKaren");
            return;
        }
        if (Time.time - _countDown >= 0.25f && !_hasAttacked)
        {
            _hasAttacked = true;
            Vector2 dirToCollider = (player.transform.position - agent.transform.position).normalized;
            player.Rb.AddForce(dirToCollider * agent.KnockbackForce, ForceMode2D.Impulse);
            player.TakeDamage(10);
        }
        if (agent.IsDead)
        {
            ServiceLocator.Get<TutorialLoopManager>().EnterDialogueEvent("TutorialEnd", true);
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
