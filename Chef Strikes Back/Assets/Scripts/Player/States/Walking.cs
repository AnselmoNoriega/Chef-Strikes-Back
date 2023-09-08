using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerWalking : StateClass<Player>
{
    private float moveSpeed = 2.3f;
    private float acceleration = 100.0f;
    private Vector2 movementAngle = new Vector2(2.0f, 1.0f);

    private Animator animator;
    private Vector2 moveDirection;
    private int direction;
    private int currentDirection;

    PlayerWalking(Animator anim)
    {
        animator = anim;
    }

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
        moveDirection = (agent.move.ReadValue<Vector2>() * movementAngle).normalized;
        currentDirection = PlayerHelper.FaceMovementDirection(animator, moveDirection, directionNames);

        if (direction != currentDirection) ChangeDirectionSpeed(agent, currentDirection);
    }

    public void FixedUpdate(Player agent)
    {
        agent.rb.AddForce(((moveDirection * moveSpeed) - agent.rb.velocity) * acceleration);
    }

    public void Exit(Player agent)
    {

    }

    private void ChangeDirectionSpeed(Player agent, int newDirection)
    {
        agent.rb.velocity = moveDirection * agent.rb.velocity.magnitude;
        direction = newDirection;
    }
}
