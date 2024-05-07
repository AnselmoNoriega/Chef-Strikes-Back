using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerStates { Idle, Walking, None }
public enum PlayerActions { None, Attacking, Throwing }
//------------------------------------------------------------------------------------------------

public class PlayerIdle : StateClass<Player>
{
    public void Enter(Player agent)
    {

    }

    public void Update(Player agent, float dt)
    {

    }

    public void FixedUpdate(Player agent)
    {
        agent.StopPlayerMovement();
    }

    public void Exit(Player agent)
    {

    }

    public void CollisionEnter2D(Player agent, Collision2D collision)
    {

    }

    public void TriggerEnter2D(Player agent, Collider2D collision)
    {

    }
}

public class PlayerNone : StateClass<Player>
{
    public void Enter(Player agent) { }

    public void Update(Player agent, float dt) { }

    public void FixedUpdate(Player agent) { }

    public void Exit(Player agent) { }

    public void CollisionEnter2D(Player agent, Collision2D collision) { }

    public void TriggerEnter2D(Player agent, Collider2D collision) { }
}

public class PlayerAttacking : StateClass<Player>
{
    private PlayerVariables _variables;
    private float _stateDuration;

    public void Enter(Player agent)
    {
        _variables = agent.Variables;
        _stateDuration = agent.Variables.AttackDuration;
        agent.Rb.velocity = Vector2.zero;
        Attack(agent.LookingDirection, agent);
        agent.PlayerAnimator.SetBool("IsAttacking", true);
    }

    public void Update(Player agent, float dt)
    {
        _stateDuration -= dt;

        if (_stateDuration <= 0.0f)
        {
            agent.ChangeAction(PlayerActions.None);
        }
    }

    public void FixedUpdate(Player agent)
    {

    }

    public void Exit(Player agent)
    {
        agent.PlayerAnimator.SetBool("IsAttacking", false);
    }

    public void CollisionEnter2D(Player agent, Collision2D collision)
    {

    }

    public void TriggerEnter2D(Player agent, Collider2D collision)
    {

    }

    public void Attack(Vector2 angle, Player player)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)player.transform.position, _variables.AttackRange);
        PlayerHelper.FaceMovementDirection(player.PlayerAnimator, angle);

        foreach (var hit in hits)
        {
            if (hit.tag == "Cops" || hit.tag == "Enemy")
            {
                var enemyAI = hit.gameObject;
                Vector2 dirToCollider = (enemyAI.transform.position - player.gameObject.transform.position).normalized;
                float angleToCollider = Vector2.Angle(angle, dirToCollider);

                if (angleToCollider <= 45.0f && hit.GetComponent<AI>())
                {
                    ServiceLocator.Get<AudioManager>().PlaySource("hit_attack");
                    enemyAI.GetComponent<AI>().Damage((int)player.Weapon.Damage);
                    enemyAI.GetComponent<AI>().Rb2d.AddForce(dirToCollider * _variables.KnockbackForce, ForceMode2D.Impulse);
                    enemyAI.GetComponent<AI>().IsHit = true;
                    return;
                }
                
            }

            ServiceLocator.Get<AudioManager>().PlaySource("miss_attack");
        }
    }
}

public class PlayerThrowing : StateClass<Player>
{
    private PlayerVariables _variables;
    private PlayerInputs _playerInputs;
    private float _throwStrength;

    public void Enter(Player agent)
    {
        _variables = agent.Variables;
        agent.Legs.SetActive(true);
        agent.PlayerAnimator.SetBool("IsThrowing", true);

        agent.Rb.velocity = Vector2.zero;
        agent.PlayerAnimator.speed -= _variables.ThrowAnimSpeed;
        _throwStrength = 0.0f;

        _playerInputs = agent.GetComponent<PlayerInputs>();
    }

    public void Update(Player agent, float dt)
    {
        var dir = _playerInputs.GetLookingDir();
        PlayerHelper.FaceMovementDirection(agent.PlayerAnimator, dir);

        if (_throwStrength <= _variables.MaxTimer)
        {
            _throwStrength += _variables.ThrowMultiplier * Time.deltaTime;
        }

        _variables.ThrowDirection = dir * _throwStrength;
    }

    public void FixedUpdate(Player agent)
    {

    }

    public void Exit(Player agent)
    {
        ServiceLocator.Get<AudioManager>().PlaySource("throw");
        agent.PlayerAnimator.SetBool("IsThrowing", false);
        _variables.ThrowDirection = Vector2.zero;
        agent.PlayerAnimator.speed += _variables.ThrowAnimSpeed;
        agent.Legs.SetActive(false);
    }

    public void CollisionEnter2D(Player agent, Collision2D collision)
    {

    }

    public void TriggerEnter2D(Player agent, Collider2D collision)
    {

    }
}