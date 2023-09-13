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
    private string[] attackDirection =
        {
        "Attack_Right", "Attack_RightTop", "Attack_Top", "Attack_LeftTop",
        "Attack_Left", "Attack_LeftBot", "Attack_Bot", "Attack_RightBot"
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

    public void Enter(Player agent)
    {
        timer = 0.5f;
        agent.rb.velocity = Vector2.zero;
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