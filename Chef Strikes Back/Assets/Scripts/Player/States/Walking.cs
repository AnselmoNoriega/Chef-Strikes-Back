using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalking : StateClass<Player>
{
    private Vector2 _moveDirection;
    private int _direction;
    private int _currentDirection;

    private AudioManager _audioManager;
    private CanvasManager _canvasManager;
    private PlayerInputs _playerInput;
    private Rigidbody2D _playerRB;

    private bool _isOnAwake = true;

    public void Enter(Player agent)
    {
        if (_isOnAwake)
        {
            _playerInput = agent.GetComponent<PlayerInputs>();
            _playerRB = agent.GetComponent<Rigidbody2D>();
            _audioManager = ServiceLocator.Get<AudioManager>();
            _canvasManager = ServiceLocator.Get<CanvasManager>();
            _isOnAwake = false;
        }

        agent.PlayerAnimator.SetBool("isWalking", true);
        agent.Legs.GetComponent<Animator>().SetBool("IsWalking", true);
        ServiceLocator.Get<Player>().IsWalking = true;
        ServiceLocator.Get<AudioManager>().PlaySource(agent.FloorSoundName);
        _canvasManager.UITransparent();
    }

    public void Update(Player agent, float dt)
    {
        if (agent.PlayerAction != PlayerActions.None && agent.PlayerAction != PlayerActions.Throwing)
        {
            agent.ChangeState(PlayerStates.Idle);
            return;
        }

        _moveDirection = _playerInput.GetMovement();

        if (_direction != _currentDirection)
        {
            ChangeDirectionSpeed(_currentDirection);
        }

        if (_moveDirection == Vector2.zero)
        {
            agent.ChangeState(PlayerStates.Idle);
        }
        else if (_moveDirection.magnitude >= 0.1f)
        {
            _currentDirection = PlayerHelper.FaceMovementDirection(agent.PlayerAnimator, _moveDirection);
            agent.LookingDirection = _moveDirection;
        }
    }

    public void FixedUpdate(Player agent)
    {
        agent.MovePlayer(_moveDirection);
    }

    public void Exit(Player agent)
    {
        agent.Legs.GetComponent<Animator>().SetBool("IsWalking", false);
        agent.GetComponent<Animator>().SetBool("isWalking", false);
        ServiceLocator.Get<CanvasManager>().UIUnTransparent();
        ServiceLocator.Get<AudioManager>().StopSource(agent.FloorSoundName);
    }

    public void CollisionEnter2D(Player agent, Collision2D collision)
    {

    }

    public void TriggerEnter2D(Player agent, Collider2D collision)
    {

    }

    private void ChangeDirectionSpeed(int newDirection)
    {
        _playerRB.velocity = _moveDirection * _playerRB.velocity.magnitude;
        _direction = newDirection;
    }
    
}
