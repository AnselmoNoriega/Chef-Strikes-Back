using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    [Header("Counts Info")]
    private int _currentHealth;
    private int _money;

    [HideInInspector, Space, Header("Attack Info")]
    public Vector2 LookingDirection;
    public int Killscount;

    [HideInInspector, Space, Header("Throw Info")]
    public Vector2 ThrowLookingDir = Vector2.zero;

    [Space, Header("MaxStats Info")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _maxRage;

    [Space, Header("World Info")] 
    public Weapon _weapon;

    [Space, Header("State Info")]
    public PlayerStates PlayerState;
    public PlayerActions PlayerAction;

    [HideInInspector] public Actions Actions;
    [HideInInspector] public Animator Animator;
    [HideInInspector] public InputAction Move;
    [HideInInspector] public Rigidbody2D Rb;
    [HideInInspector] public InputAction Mouse;

    private StateMachine<Player> _stateMachine;
    private StateMachine<Player> _actionState;

    private InputControls _inputManager;
    private bool _initialized = false;
    [Space, Header("Player Got Hit Animation")]
    [SerializeField] SpriteRenderer playerImage;
    [SerializeField]private int _FlashingTime;
    public void Initialize()
    {
        _stateMachine = new StateMachine<Player>(this);
        _actionState = new StateMachine<Player>(this);
        _weapon = new Weapon(0);

        AddStates();
        _stateMachine.ChangeState(0);
        _actionState.ChangeState(0);

        Actions = GetComponent<Actions>();
        Animator = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody2D>();

        _currentHealth = _maxHealth;
        ServiceLocator.Get<CanvasManager>().SetMaxHealth(_maxHealth);

        _initialized = true;
    }

    private void OnEnable()
    {
        _inputManager = new InputControls();
        Move = _inputManager.Player.Move;
        Move?.Enable();
    }

    private void OnDisable()
    {
        Move?.Disable();
    }

    public void Update()
    {
        if (!_initialized)
        {
            return;
        }

        if (Move.ReadValue<Vector2>() != Vector2.zero && PlayerState == PlayerStates.Idle)
        {
            ChangeState(PlayerStates.Walking);
        }

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

    private void AddStates()
    {
        _stateMachine.AddState<PlayerIdle>();
        _stateMachine.AddState<PlayerWalking>();
        
        _actionState.AddState<PlayerNone>();
        _actionState.AddState<PlayerAttacking>();
        _actionState.AddState<PlayerThrowing>();
    }

    public void TakeDamage(int amt)
    {
        _currentHealth -= amt;
        
        if(_currentHealth <= 0)
        {
            ServiceLocator.Get<SceneControl>().ChangeScene("DeathScene");
            return;
        }
        StartCoroutine(SpriteFlashing());
        ServiceLocator.Get<CanvasManager>().AddTooHealthSlider(-amt);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Loot")
        {
            _money += 10;
            ServiceLocator.Get<GameManager>().MoneyGrabed();
            ServiceLocator.Get<CanvasManager>().ChangeMoneyValue(_money);
            ServiceLocator.Get<AudioManager>().PlaySource("money");
            Destroy(collision.gameObject);
        }
        
        
    }

    public int GetKillsCount()
    {
        return Killscount;
    }

    public int GetDailyEarnings()
    {
        return _money;
    }

    private IEnumerator SpriteFlashing()
    {
        for(int i = 0; i < _FlashingTime;i++)
        {
            playerImage.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            playerImage.color = new Color(0.8735808f, 0.4141153f, 0.8867924f, 1.0f);
        }
        
    }
}