using UnityEngine;

public class PlayerWalking : StateClass<Player>
{
    private float _moveSpeed = 2.3f;
    private float _throwMoveSpeed = 0.8f;
    private float _acceleration = 100.0f;
    private Vector2 _movementAngle = new Vector2(2.0f, 1.0f);

    private Vector2 _moveDirection;
    private int _direction;
    private int _currentDirection;

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

        _moveDirection = (agent.Move.ReadValue<Vector2>() * _movementAngle).normalized;

        if (_direction != _currentDirection) ChangeDirectionSpeed(agent, _currentDirection);

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
            _currentDirection = PlayerHelper.FaceMovementDirection(agent.Animator, _moveDirection);
            agent.LookingDirection = _moveDirection;
        }
    }

    public void FixedUpdate(Player agent)
    {
        Vector2 moveSpeed;

        if (agent.PlayerAction == PlayerActions.Throwing)
        {
            moveSpeed = _moveDirection * _throwMoveSpeed * agent.SpeedBoost;
        }
        else
        {
            moveSpeed = _moveDirection * _moveSpeed * agent.SpeedBoost;
        }

        agent.Rb.AddForce((moveSpeed + agent._floorSpeed - agent.Rb.velocity) * _acceleration);
    }
    private void FaceDirectionForIdle(Player agent)
    {
        if (_moveDirection.magnitude <= 0.1f)
        {
            return;
        }
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
        agent.Rb.velocity = _moveDirection * agent.Rb.velocity.magnitude;
        _direction = newDirection;
    }
}
