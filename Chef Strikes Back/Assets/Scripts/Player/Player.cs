using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Counts Info")]
    public float currentHealth;
    public float currentRage;
    private int money;

    [HideInInspector, Space, Header("Attack Info")]
    public Vector2 lookingDirection;

    [Space, Header("MaxStats Info")]
    public float maxHealth;
    public float MaxRage;


    [Space, Header("World Info"), SerializeField]
    private SceneControl sceneControl;
    public Weapon _weapon;
    [SerializeField]
    private Slider rageBar;
    [SerializeField]
    private Slider healthBar;

    [Space, Header("State Info")]
    public PlayerStates playerState;
    public PlayerActions playerAction;
    public PlayerStage playerMode;

    [HideInInspector]
    public Actions actions;
    [HideInInspector] 
    public Animator animator;
    [HideInInspector] 
    public InputAction move;
    [HideInInspector] 
    public Rigidbody2D rb;
    [HideInInspector]
    public InputAction mouse;

    private StateMachine<Player> stateMachine;
    private StateMachine<Player> actionState;
    private StateMachine<Player> moodState;

    [Space, Header("Rage Info")]
    public GameObject vignette;

    private InputControls inputManager;
    private bool _initialized = false;

    public void Initialize()
    {
        stateMachine = new StateMachine<Player>(this);
        actionState = new StateMachine<Player>(this);
        moodState = new StateMachine<Player>(this);
        _weapon = new Weapon(0);

        AddStates();
        stateMachine.ChangeState(0);
        actionState.ChangeState(0);
        moodState.ChangeState(0);

        actions = GetComponent<Actions>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        rageBar.maxValue = MaxRage;
        currentRage = 0;
        _initialized = true;
    }

    private void OnEnable()
    {
        inputManager = new InputControls();
        move = inputManager.Player.Move;
        move?.Enable();
    }

    private void OnDisable()
    {
        move?.Disable();
    }

    public void Update()
    {
        if (!_initialized)
        {
            return;
        }

        rageBar.value = currentRage;
        healthBar.value = currentHealth / maxHealth;

        if (move.ReadValue<Vector2>() != Vector2.zero && playerState == PlayerStates.Idle)
        {
            ChangeState(PlayerStates.Walking);
        }

        if (money >= 100)
        {
            sceneControl.switchToWinScene();
        }

        stateMachine.Update(Time.deltaTime);
        actionState.Update(Time.deltaTime);
        moodState.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (!_initialized)
        {
            return;
        }

        stateMachine.FixedUpdate();
        actionState.FixedUpdate();
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            sceneControl.switchToGameOverScene();
        }
    }

    public void collectMoney(int amount)
    {
        money += amount;
    }

    public void ChangeState(PlayerStates state)
    {
        if (playerState != state)
        {
            playerState = state;
            stateMachine.ChangeState((int)state);
        }
    }

    public void ChangeAction(PlayerActions state)
    {
        if (playerAction != state)
        {
            playerAction = state;
            actionState.ChangeState((int)state);
        }
    }

    public void ChangeMood(PlayerStage state)
    {
        if (playerMode != state)
        {
            playerMode = state;
            moodState.ChangeState((int)state);
        }
    }

    private void AddStates()
    {
        stateMachine.AddState<PlayerIdle>();
        stateMachine.AddState<PlayerWalking>();

        actionState.AddState<PlayerNone>();
        actionState.AddState<PlayerAttacking>();
        actionState.AddState<PlayerThrowing>();

        moodState.AddState<NormalMode>();
        moodState.AddState<RageMode>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Loot")
        {
            ServiceLocator.Get<GameLoopManager>().money += 10;
            collision.gameObject.SetActive(false);
        }
    }
}