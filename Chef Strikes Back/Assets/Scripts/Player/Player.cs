using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Properties")]
    private SpriteRenderer _playerSprite;

    [Header("Counts Info")]
    private int _currentHealth;
    public int Money { get; private set; }
    public int KillCount { get; private set; }

    [Space, Header("Attack Info")]
    [HideInInspector] public Weapon Weapon;
    [HideInInspector] public Vector2 LookingDirection;

    [Space, Header("World Info")]
    public PlayerVariables Variables;
    private Vector2 _floorSpeed;

    [Space, Header("State Info")]
    public PlayerStates PlayerState;
    public PlayerActions PlayerAction;

    private StateMachine<Player> _stateMachine;
    private StateMachine<Player> _actionState;

    [Space, Header("Conections")]
    private CanvasManager _canvasManager;
    private GameManager _gameManager;
    private AudioManager _audioManager;

    public Rigidbody2D Rb { get; private set; }
    public Animator PlayerAnimator { get; private set; }
    
    private bool _initialized = false;

    public void Initialize()
    {
        _playerSprite = GetComponent<SpriteRenderer>();
        PlayerAnimator = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody2D>();

        _stateMachine = new StateMachine<Player>(this);
        _actionState = new StateMachine<Player>(this);
        Weapon = new Weapon(0);

        _stateMachine.AddState<PlayerIdle>();
        _stateMachine.AddState<PlayerWalking>();

        _actionState.AddState<PlayerNone>();
        _actionState.AddState<PlayerAttacking>();
        _actionState.AddState<PlayerThrowing>();

        _stateMachine.ChangeState(0);
        _actionState.ChangeState(0);

        _initialized = true;

        _currentHealth = Variables.MaxHealth;

        _gameManager = ServiceLocator.Get<GameManager>();
        _canvasManager = ServiceLocator.Get<CanvasManager>();
        _audioManager = ServiceLocator.Get<AudioManager>();

        _canvasManager.SetMaxHealth(Variables.MaxHealth);
    }

    public void Update()
    {
        if (!_initialized)
        {
            return;
        }

        Variables.SpeedBoostTimer();

        CheckFloorType();

        _stateMachine.Update(Time.deltaTime);
        _actionState.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (!_initialized)
        {
            return;
        }

        _stateMachine.FixedUpdate();
        _actionState.FixedUpdate();
    }

    public void ChangeState(PlayerStates state)
    {
        if (PlayerState != state)
        {
            PlayerState = state;
            _stateMachine.ChangeState((int)state);
        }
    }

    public void ChangeAction(PlayerActions state)
    {
        if (PlayerAction != state)
        {
            PlayerAction = state;
            _actionState.ChangeState((int)state);
        }
    }

    public void TakeDamage(int amt)
    {
        _currentHealth -= amt;

        if (_currentHealth <= 0)
        {
            _gameManager.SetKillCount(KillCount);
            ServiceLocator.Get<SceneControl>().ChangeScene("DeathScene");
            return;
        }
        StartCoroutine(SpriteFlashing());
        _canvasManager.AddTooHealthSlider(-amt);
    }

    public void MakeETransfer()
    {
        Money += 10;
        _gameManager.MoneyGrabed();
        _canvasManager.ChangeMoneyValue(Money);
        _audioManager.PlaySource("money");
    }

    public void MovePlayer(Vector2 moveDirection)
    {
        Vector2 moveSpeed = moveDirection * Variables.SpeedBoost;

        if (PlayerAction == PlayerActions.Throwing)
        {
            moveSpeed *= Variables.ThrowMoveSpeed;
        }
        else
        {
            moveSpeed *= Variables.MoveSpeed;
        }

        moveSpeed += _floorSpeed - Rb.velocity;

        Rb.AddForce(moveSpeed * Variables.PlayerAcceleration);
    }

    public void StopPlayerMovement()
    {
        Rb.AddForce((-Rb.velocity + _floorSpeed) * Variables.PlayerAcceleration);
    }

    public void AddKillCount()
    {
        ++KillCount;
    }

    private IEnumerator SpriteFlashing()
    {
        for (int i = 0; i < Variables.FlashingTime; i++)
        {
            _playerSprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            _playerSprite.color = Color.white;
        }
    }

    private void CheckFloorType()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.25f);

        foreach (Collider2D hit in hits)
        {
            var floor = hit.GetComponent<SpeedyTile>();
            if (floor && floor.GetSpeed() != _floorSpeed)
            {
                _floorSpeed = floor.GetSpeed();
                return;
            }
            else if (floor)
            {
                return;
            }
        }

        if (_floorSpeed != Vector2.zero)
        {
            _floorSpeed = Vector2.zero;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}