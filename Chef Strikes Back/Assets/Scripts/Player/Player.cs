using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Properties")]
    public GameObject Legs;
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
    public string FloorSoundName = "";

    [Space, Header("State Info")]
    public PlayerStates PlayerState;
    public PlayerActions PlayerAction;
    public bool IsWalking = false;
    public bool GotDamage = false;
    private bool _isDead = false;
    
    private StateMachine<Player> _stateMachine;
    private StateMachine<Player> _actionState;
    [Space, Header("Conections")]
    private CanvasManager _canvasManager;
    private GameManager _gameManager;

    [Header("Audio")]
    private AudioManager _audioManager;
    [SerializeField] private string[] _hitSound = { "C_Hit_00", "C_Hit_01", "C_Hit_02", "C_Hit_03", "C_Hit_04" };
    [SerializeField] private string[] _deathSound = { "C-Death_00", "C-Death_01"};
    [SerializeField] private string[] _bumpSound = { "C_Bump-Player_00", "C_Bump-Player_01", "C_Bump-Player_02", "C_Bump-Player_03", "C_Bump-Player_04" };

    public Rigidbody2D Rb { get; private set; }
    public Animator PlayerAnimator { get; private set; }

    private bool _initialized = false;
    public bool shouldNotMove = false;

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

        gameObject.SetActive(false);
        gameObject.SetActive(true);
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
        if (PlayerState != state && !_isDead)
        {
            PlayerState = state;
            _stateMachine.ChangeState((int)state);
        }
    }

    public void ChangeAction(PlayerActions state)
    {
        if (PlayerAction != state && !_isDead)
        {
            PlayerAction = state;
            _actionState.ChangeState((int)state);
        }
    }

    public void TakeDamage(int amt)
    {
        _currentHealth -= amt;
        GotDamage = true;
        if (_currentHealth <= 0)
        {
            StartCoroutine(KillPlayer());
            return;
        }
        StartCoroutine(SpriteFlashing());
        _canvasManager.AddTooHealthSlider(-amt);



        if (_hitSound.Length > 0)
        {
            string randomSound = _hitSound[Random.Range(0, _hitSound.Length)];
            Debug.Log($"Playing sound: {randomSound}");
            _audioManager.PlaySource(randomSound);
        }
        else
        {
            Debug.LogError("Hit sound array is empty!");
        }

    }

    public void MakeETransfer()
    {
        Money += 10;
        _gameManager.MoneyGrabed();
        _canvasManager.ChangeMoneyValue(Money);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            string randomSound = _bumpSound[Random.Range(0, _bumpSound.Length)];
            _audioManager.PlaySource(randomSound);
        }
    }

    public void AddKillCount()
    {
        ++KillCount;
    }

    private IEnumerator SpriteFlashing()
    {
        for (int i = 0; i < Variables.FlashingTime; i++)
        {

            PlayerAnimator.Play("Damage_" + PlayerHelper.FaceMovementDirection(PlayerAnimator, LookingDirection));
            yield return new WaitForSeconds(0.1f);
            PlayerAnimator.Play("Idle_" + PlayerHelper.FaceMovementDirection(PlayerAnimator, LookingDirection));
        }
        GotDamage = false;
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

    private IEnumerator KillPlayer()
    {
        string randomSound = _deathSound[Random.Range(0, _deathSound.Length)];
        _audioManager.PlaySource(randomSound);
        _gameManager.SetKillCount(KillCount);
        ChangeState(PlayerStates.Idle);
        ChangeAction(PlayerActions.None);
        _isDead = true;
        PlayerAnimator.Play("Player_Death");

        yield return new WaitForSeconds(3);

        ServiceLocator.Get<SceneControl>().ChangeScene("DeathScene");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}