using UnityEngine;

public class PlayerWalking : StateClass<Player>
{
    private float moveSpeed = 2.3f;
    private float throwMoveSpeed = 0.8f;
    private float acceleration = 100.0f;
    private Vector2 movementAngle = new Vector2(2.0f, 1.0f);

    private Vector2 moveDirection;
    private int direction;
    private int currentDirection;

    public void Enter(Player agent)
    {
        agent.GetComponent<Animator>().SetBool("isWalking", true);
        ServiceLocator.Get<AudioManager>().PlaySource("walk");
        ServiceLocator.Get<CanvasManager>().UITransparent();
    }

    public void Update(Player agent, float dt)
    {
        if (agent.PlayerAction != PlayerActions.None && agent.PlayerAction != PlayerActions.Throwing)
        {
            agent.ChangeState(PlayerStates.Idle);
            return;
        }

        moveDirection = (agent.Move.ReadValue<Vector2>() * movementAngle).normalized;

        if (direction != currentDirection) ChangeDirectionSpeed(agent, currentDirection);

        if (agent.Move.ReadValue<Vector2>() == Vector2.zero)
        {
            agent.ChangeState(PlayerStates.Idle);
            FaceDirectionForIdle(agent);
            ServiceLocator.Get<AudioManager>().StopSource("walk");

        }
        else
        {
            if (!ServiceLocator.Get<AudioManager>().IsPlaying("walk"))
            {
                ServiceLocator.Get<AudioManager>().PlaySource("walk");
            }
            currentDirection = PlayerHelper.FaceMovementDirection(agent.Animator, moveDirection);
            agent.LookingDirection = moveDirection;
        }
    }

    public void FixedUpdate(Player agent)
    {
        if (agent.PlayerAction == PlayerActions.Throwing)
        {
            agent.Rb.AddForce(((moveDirection * (throwMoveSpeed)) + agent.FloorSpeed - agent.Rb.velocity) * acceleration);
        }
        else
        {
            agent.Rb.AddForce(((moveDirection * moveSpeed) + agent.FloorSpeed - agent.Rb.velocity) * acceleration);
        }
    }
    private void FaceDirectionForIdle(Player agent)
    {
        Vector2 IdleDirection;

        if (Mathf.Abs(agent.Rb.velocity.x) > Mathf.Abs(agent.Rb.velocity.y))
        {
            IdleDirection = new Vector2(Mathf.Sign(agent.Rb.velocity.x), 0);
        }
        else
        {
            IdleDirection = new Vector2(0, Mathf.Sign(agent.Rb.velocity.y));
        }
        agent.LookingDirection = IdleDirection;
        PlayerHelper.FaceMovementDirection(agent.Animator, IdleDirection);
    }
    public void Exit(Player agent)
    {
        agent.GetComponent<Animator>().SetBool("isWalking", false);
        ServiceLocator.Get<CanvasManager>().UIUnTransparent();
    }

    public void CollisionEnter2D(Player agent, Collision2D collision)
    {

    }

    public void TriggerEnter2D(Player agent, Collider2D collision)
    {

    }

    private void ChangeDirectionSpeed(Player agent, int newDirection)
    {
        agent.Rb.velocity = moveDirection * agent.Rb.velocity.magnitude;
        direction = newDirection;
    }
}
