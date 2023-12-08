using UnityEngine;

public class PlayerWalking : StateClass<Player>
{
    private float moveSpeed = 2.3f;
    private float rageSpeed = 3.0f;
    private float acceleration = 100.0f;
    private Vector2 movementAngle = new Vector2(2.0f, 1.0f);

    private Vector2 moveDirection;
    private int direction;
    private int currentDirection;

    public void Enter(Player agent)
    {
        ServiceLocator.Get<AudioManager>().PlaySource("walk");
    }

    public void Update(Player agent, float dt)
    {
        

        if (agent.PlayerMode == PlayerStage.Normal && agent.PlayerAction != PlayerActions.None)
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
        if (agent.PlayerMode != PlayerStage.Rage)
        {
            agent.Rb.AddForce(((moveDirection * moveSpeed) - agent.Rb.velocity) * acceleration);
        }
        else
        {
            agent.Rb.AddForce(((moveDirection * rageSpeed) - agent.Rb.velocity) * acceleration);
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
