using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Counts Info")]
    private int _currentHealth;
    private int _currentRage;
    private int _money;

    [HideInInspector, Space, Header("Attack Info")]
    public Vector2 LookingDirection;

    [Space, Header("MaxStats Info")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _maxRage;

    [Space, Header("World Info")] 
    public Weapon _weapon;

    [Space, Header("State Info")]
    public PlayerStates PlayerState;
    public PlayerActions PlayerAction;
    public PlayerStage PlayerMode;

    [HideInInspector] public Actions Actions;
    [HideInInspector] public Animator Animator;
    [HideInInspector] public InputAction Move;
    [HideInInspector] public Rigidbody2D Rb;
    [HideInInspector] public InputAction Mouse;

    private StateMachine<Player> _stateMachine;
    private StateMachine<Player> _actionState;
    private StateMachine<Player> _moodState;

    private InputControls _inputManager;
    private bool _initialized = false;

    public void Initialize()
    {
        _stateMachine = new StateMachine<Player>(this);
        _actionState = new StateMachine<Player>(this);
        _moodState = new StateMachine<Player>(this);
        _weapon = new Weapon(0);

        AddStates();
        _stateMachine.ChangeState(0);
        _actionState.ChangeState(0);
        _moodState.ChangeState(0);

        Actions = GetComponent<Actions>();
        Animator = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody2D>();

        _currentHealth = _maxHealth;
        ServiceLocator.Get<CanvasManager>().SetMaxHealth(_maxHealth);
        _currentRage = 0;
        ServiceLocator.Get<CanvasManager>().SetMaxRage(_maxRage);

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
        _moodState.Update(Time.deltaTime);
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

    public void ChangeMood(PlayerStage state)
    {
        if (PlayerMode != state)
        {   
            PlayerMode = state;
            _moodState.ChangeState((int)state);
        }
    }

    private void AddStates()
    {
        _stateMachine.AddState<PlayerIdle>();
        _stateMachine.AddState<PlayerWalking>();
        
        _actionState.AddState<PlayerNone>();
        _actionState.AddState<PlayerAttacking>();
        _actionState.AddState<PlayerThrowing>();
        
        _moodState.AddState<NormalMode>();
        _moodState.AddState<RageMode>();
    }

    public void TakeDamage(int amt)
    {
        _currentHealth -= amt;
        if(_currentHealth <= 0)
        {
            ServiceLocator.Get<SceneControl>().GoToEndScene();
            return;
        }

        ServiceLocator.Get<CanvasManager>().AddTooHealthSlider(-amt);
    }

    public void ExitRageMode()
    {
        _currentRage = 0;
        ChangeMood(PlayerStage.Normal);
        ServiceLocator.Get<CanvasManager>().ChangeRageSliderValue(0);
    }

    public void TakeRage(int amt)
    {
        _currentRage += amt;
        if(_currentRage >= _maxRage)
        {
            ServiceLocator.Get<GameLoopManager>().SetRageMode(true);
            ChangeMood(PlayerStage.Rage);
        }
        ServiceLocator.Get<CanvasManager>().AddTooRageSlider(amt);
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
}