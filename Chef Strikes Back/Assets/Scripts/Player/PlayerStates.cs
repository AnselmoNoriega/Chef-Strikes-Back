using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public enum PlayerStates { Idle, Walking, Attacking, Throwing, None }
public enum PlayerStage { Normal, Rage, None }
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

    }

    public void Exit(Player agent)
    {

    }
}

public class PlayerWalking : StateClass<Player>
{
    private float moveSpeed;
    private Rigidbody2D rb;
    private float acceleration;
    private Vector2 movementAngle;

    private InputControls inputManager;
    private InputAction move;
    private Vector2 moveDirection;
    private Animator animator;
    private int direction;

    string[] directionNames =
        {
        "Idle_Right", "Idle_RightTop", "Idle_Front", "Idle_LeftTop",
        "Idle_Left", "Idle_LeftBot", "Idle_Bot", "Idle_RightBot"
    };
    string[] attackDirection =
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

    }

    public void Exit(Player agent)
    {

    }
}

public class PlayerAttacking : StateClass<Player>
{
    public void Enter(Player agent)
    {

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

public class PlayerThrowing : StateClass<Player>
{
    public void Enter(Player agent)
    {

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