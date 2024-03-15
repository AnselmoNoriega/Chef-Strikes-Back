using UnityEngine;

public enum PlayerStates { Idle, Walking, None }
public enum PlayerActions { None, Attacking, Throwing }
//------------------------------------------------------------------------------------------------

public class PlayerIdle : StateClass<Player>
{
    private float acceleration = 100.0f;

    public void Enter(Player agent)
    {

    }

    public void Update(Player agent, float dt)
    {

    }

    public void FixedUpdate(Player agent)
    {
        agent.Rb.AddForce((-agent.Rb.velocity + agent.FloorSpeed) * acceleration);
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
    float timer;

    public void Enter(Player agent)
    {
        timer = 0.1f;
        agent.Rb.velocity = Vector2.zero;
        Attack(agent.LookingDirection, agent);
        agent.Animator.SetBool("IsAttacking", true);
    }

    public void Update(Player agent, float dt)
    {
        timer -= dt;

        if (timer <= 0.0f)
        {
            agent.ChangeAction(PlayerActions.None);
        }
    }

    public void FixedUpdate(Player agent)
    {

    }

    public void Exit(Player agent)
    {
        agent.Animator.SetBool("IsAttacking", false);
    }

    public void CollisionEnter2D(Player agent, Collision2D collision)
    {

    }

    public void TriggerEnter2D(Player agent, Collider2D collision)
    {

    }

    public void Attack(Vector2 angle, Player player)
    {
        
        Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)player.transform.position, player.GetComponent<Actions>().PlayerAttackRange);
        PlayerHelper.FaceMovementDirection(player.Animator, angle);

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
                    enemyAI.GetComponent<AI>().Damage((int)player._weapon.Damage);
                    enemyAI.GetComponent<AI>().Rb2d.AddForce(dirToCollider * player.KnockbackForce, ForceMode2D.Impulse);
                    return;
                }
                else if(angleToCollider <= 45.0f && hit.GetComponent<Cops>())
                {
                    ServiceLocator.Get<AudioManager>().PlaySource("hit_attack");
                    enemyAI.GetComponent<Cops>().Damage((int)player._weapon.Damage);
                    enemyAI.GetComponent<Cops>().Rb2d.AddForce(dirToCollider * enemyAI.GetComponent<Cops>().knockbackForce,ForceMode2D.Impulse);
                    enemyAI.GetComponent<Cops>().isHit = true;
                    return;
                }
            }

            ServiceLocator.Get<AudioManager>().PlaySource("miss_attack");
        }
    }
}

public class PlayerThrowing : StateClass<Player>
{
    private Vector3 offset = new Vector3(0, 0.35f, 0);
    private float _timer;
    private float _throwMultiplier = 1.0f;
    private float _maxTimer = 1.5f;
    private float _throwAnimSpeed = 0.5f;

    public void Enter(Player agent)
    {
        agent.Rb.velocity = Vector2.zero;
        agent.Animator.speed -= _throwAnimSpeed;
        _timer = 0;
    }

    public void Update(Player agent, float dt)
    {
        var mousePos = Camera.main.ScreenToWorldPoint(agent.Mouse.ReadValue<Vector2>());
        var dir = (mousePos - (agent.transform.position + offset));
        PlayerHelper.FaceMovementDirection(agent.Animator, dir);

        if (_timer <= _maxTimer)
        {
            _timer += _throwMultiplier * Time.deltaTime;
        }
        agent.ThrowLookingDir = dir.normalized * _timer;
    }

    public void FixedUpdate(Player agent)
    {

    }

    public void Exit(Player agent)
    {
        ServiceLocator.Get<AudioManager>().PlaySource("throw");
        agent.ThrowLookingDir = Vector2.zero;
        agent.Animator.speed += _throwAnimSpeed;
    }

    public void CollisionEnter2D(Player agent, Collision2D collision)
    {

    }

    public void TriggerEnter2D(Player agent, Collider2D collision)
    {

    }
}