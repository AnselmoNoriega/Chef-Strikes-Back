using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public enum PlayerStates { Idle, Walking, None }
public enum PlayerActions { None, Attacking, Throwing }
public enum PlayerStage { Normal, Rage, None }
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
        agent.rb.AddForce((- agent.rb.velocity) * acceleration);
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
        agent.rb.velocity = Vector2.zero;
        Attack(agent.attackDir, agent);
        agent.animator.SetBool("IsAttacking", true);
    }

    public void Update(Player agent, float dt)
    {
        timer -= dt;

        if(timer <= 0.0f) 
        {
            agent.ChangeAction(PlayerActions.None);
        }
    }

    public void FixedUpdate(Player agent)
    {

    }

    public void Exit(Player agent)
    {
        agent.animator.SetBool("IsAttacking", false);
    }

    public void CollisionEnter2D(Player agent, Collision2D collision)
    {

    }

    public void TriggerEnter2D(Player agent, Collider2D collision)
    {

    }

    public void Attack(Vector2 mousePos, Player player)
    {
        var rayOrigin = new Vector2(player.transform.position.x, player.transform.position.y + 0.35f);
        var attackDirection = (mousePos - rayOrigin).normalized;

        RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, attackDirection, player._weapon.Range); 
        PlayerHelper.FaceMovementDirection(player.animator, attackDirection);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Player"))
            {
                continue;
            }

            var enemyAI = hit.collider.GetComponent<AI>();

            if (enemyAI)
            {
                if (enemyAI.stateManager.CurrentState == (int)AIState.Rage)
                {
                    enemyAI.health -= Mathf.RoundToInt(player._weapon.Damage);
                }
            }

            var foodPile = hit.collider.GetComponent<FoodPile>();

            if (foodPile != null)
            {
                foodPile.Hit(1);
            }
        }
    }
}

public class PlayerThrowing : StateClass<Player>
{
    private Vector3 offset = new Vector3(0, 0.35f, 0);

    public void Enter(Player agent)
    {
        agent.rb.velocity = Vector2.zero;
    }

    public void Update(Player agent, float dt)
    {
        var mousePos = Camera.main.ScreenToWorldPoint(agent.mouse.ReadValue<Vector2>());
        var dir = (mousePos - (agent.transform.position + offset));
        PlayerHelper.FaceMovementDirection(agent.animator, dir);
    }

    public void FixedUpdate(Player agent)
    {

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


//------------------------------------------------------------------------------------------------

public class NormalMode : StateClass<Player>
{
    public void Enter(Player agent)
    {

    }

    public void Update(Player agent, float dt)
    {
        if(GameManager.Instance.rageMode)
        {
            agent.ChangeMood(PlayerStage.Rage);
        }
    }

    public void FixedUpdate(Player agent)
    {

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

public class RageMode : StateClass<Player>
{
    public void Enter(Player agent)
    {
        agent.vignette.SetActive(true);
        agent.actions.DropItem();
    }

    public void Update(Player agent, float dt)
    {
        if (!GameManager.Instance.rageMode)
        {
            agent.ChangeMood(PlayerStage.Normal);
        }
    }

    public void FixedUpdate(Player agent)
    {

    }

    public void Exit(Player agent)
    {
        agent.vignette.SetActive(false);
    }

    public void CollisionEnter2D(Player agent, Collision2D collision)
    {

    }

    public void TriggerEnter2D(Player agent, Collider2D collision)
    {

    }
}