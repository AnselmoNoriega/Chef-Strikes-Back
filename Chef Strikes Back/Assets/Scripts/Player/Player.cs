using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Player : MonoBehaviour
{
    [Header("Counts Info")]
    public float currentHealth;
    public float currentRage;
    private int enemyKills;
    private int money;

    [HideInInspector, Space, Header("Attack Info")]
    public Vector2 attackDir;

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
    public PlayerStage playerMode;

    [HideInInspector] 
    public Animator animator;
    [HideInInspector] 
    public InputAction move;
    [HideInInspector] 
    public Rigidbody2D rb;
    [HideInInspector]
    public InputAction mouse;

    private StateMachine<Player> stateMachine;
    private StateMachine<Player> moodState;

    [Space, Header("Rage Info")]
    public GameObject vignette;

    private InputControls inputManager;

    public void Awake()
    {
        stateMachine = new StateMachine<Player>(this);
        moodState = new StateMachine<Player>(this);
        inputManager = new InputControls();
        inputManager = new InputControls();
        _weapon = new Weapon(0);
        move = inputManager.Player.Move;
    }

    private void OnEnable()
    {
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    private void Start()
    {
        AddStates();
        stateMachine.ChangeState(0);
        moodState.ChangeState(0);
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        rageBar.maxValue = MaxRage;
        currentRage = 0;
    }

    public void Update()
    {
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
        moodState.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    public void TakeDamage(int damageAmount)
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
        stateMachine.AddState<PlayerAttacking>();
        stateMachine.AddState<PlayerThrowing>();

        moodState.AddState<NormalMode>();
        moodState.AddState<RageMode>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Loot")
        {
            GameManager.Instance.money += 10;
            collision.gameObject.SetActive(false);
        }
    }
}