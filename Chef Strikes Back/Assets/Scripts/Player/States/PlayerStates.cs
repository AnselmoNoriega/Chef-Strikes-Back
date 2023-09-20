using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public enum PlayerStates { Idle, Walking, Attacking, Throwing, None }
public enum PlayerStage { Normal, Rage, None }
//------------------------------------------------------------------------------------------------

public class PlayerIdle : StateClass<Player>
{
    private float moveSpeed = 2.3f;
    private float acceleration = 100.0f;

    private string[] directionNames =
        {
        "Idle_Right", "Idle_RightTop", "Idle_Front", "Idle_LeftTop",
        "Idle_Left", "Idle_LeftBot", "Idle_Bot", "Idle_RightBot"
    };

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
}

public class PlayerAttacking : StateClass<Player>
{
    float timer;

    private string[] attackDirections =
        {
        "Attack_Right", "Attack_RightTop", "Attack_Top", "Attack_LeftTop",
        "Attack_Left", "Attack_LeftBot", "Attack_Bot", "Attack_RightBot"
    };

    public void Enter(Player agent)
    {
        timer = 0.5f;
        agent.rb.velocity = Vector2.zero;
        Attack(agent.attackDir, agent);
    }

    public void Update(Player agent, float dt)
    {
        timer -= dt;

        if(timer <= 0.0f) 
        {
            agent.ChangeState(PlayerStates.Idle);
        }
    }

    public void FixedUpdate(Player agent)
    {

    }

    public void Exit(Player agent)
    {

    }

    public void Attack(Vector2 mousePos, Player player)
    {
        var rayOrigin = new Vector2(player.transform.position.x, player.transform.position.y + 0.35f);
        var attackDirection = (mousePos - rayOrigin).normalized;

        RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, attackDirection, player._weapon.Range);
        player.animator.Play(PlayerHelper.GetDirection(attackDirection, attackDirections));

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Player"))
            {
                continue;
            }

            var enemyAI = hit.collider.GetComponent<AI>();

            if (enemyAI)
            {
                if (enemyAI.stateManager.CurrentAIState == StateManager.AIState.Rage)
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
    public void Enter(Player agent)
    {
        agent.rb.velocity = Vector2.zero;
    }

    public void Update(Player agent, float dt)
    {

    }

    public void FixedUpdate(Player agent)
    {

    }

    public void Exit(Player agent)
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
}

public class RageMode : StateClass<Player>
{
    public void Enter(Player agent)
    {
        agent.vignette.SetActive(true);
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
}