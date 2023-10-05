using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerWalking : StateClass<Player>
{
    private float moveSpeed = 2.3f;
    private float acceleration = 100.0f;
    private Vector2 movementAngle = new Vector2(2.0f, 1.0f);

    private Vector2 moveDirection;
    private int direction;
    private int currentDirection;

    public void Enter(Player agent)
    {

    }

    public void Update(Player agent, float dt)
    {
        moveDirection = (agent.move.ReadValue<Vector2>() * movementAngle).normalized;

        if (direction != currentDirection) ChangeDirectionSpeed(agent, currentDirection);

        if (agent.move.ReadValue<Vector2>() == Vector2.zero)
        {
            agent.ChangeState(PlayerStates.Idle);
        }
        else
        {
            currentDirection = PlayerHelper.FaceMovementDirection(agent.animator, moveDirection);
        }
    }

    public void FixedUpdate(Player agent)
    {
        agent.rb.AddForce(((moveDirection * moveSpeed) - agent.rb.velocity) * acceleration);
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

    private void ChangeDirectionSpeed(Player agent, int newDirection)
    {
        agent.rb.velocity = moveDirection * agent.rb.velocity.magnitude;
        direction = newDirection;
    }
}
