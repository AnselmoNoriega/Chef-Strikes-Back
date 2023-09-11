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

    [Space, Header("Attack Info")]
    public float attackCooldown;
    public bool isCoolingDown;

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

    [HideInInspector] public Animator animator;
    [HideInInspector] public InputAction move;
    [HideInInspector] public Rigidbody2D rb;

    private StateMachine<Player> stateMachine;
    private StateMachine<Player> moodState;

    [Space, Header("Rage Info")]
    public GameObject vignette;

    private InputControls inputManager;

    private string[] attackDirections =
        {
        "Attack_Right", "Attack_RightTop", "Attack_Top", "Attack_LeftTop",
        "Attack_Left", "Attack_LeftBot", "Attack_Bot", "Attack_RightBot"
    };

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
        currentRage = 0;
        currentHealth = maxHealth;
        rageBar.maxValue = MaxRage;
    }

    public void Update()
    {
        currentRage = GameManager.Instance.RageValue;
        rageBar.value = currentRage;
        healthBar.value = currentHealth / maxHealth;

        if (move.ReadValue<Vector2>() != Vector2.zero && playerState == PlayerStates.Idle)
        {
            ChangeState(PlayerStates.Walking);
        }

        if (isCoolingDown)
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0)
            {
                isCoolingDown = false;
            }
        }
        if (money >= 100)
        {
            sceneControl.switchToWinScene();
        }
        stateMachine.Update(Time.deltaTime);
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

    public void Attack(Vector2 mousePos)
    {
        if (isCoolingDown)
        {
            return;
        }

        ChangeState(PlayerStates.Attacking);

        var rayOrigin = new Vector2(transform.position.x, transform.position.y + 0.35f);
        var attackDirection = (mousePos - rayOrigin).normalized;

        RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, attackDirection, _weapon.Range);
        animator.Play(PlayerHelper.GetDirection(attackDirection, attackDirections));

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Player"))
            {
                continue;
            }

            var enemyAI = hit.collider.GetComponent<AI>();

            if (enemyAI)
            {
                if (enemyAI.stateManager.CurrentAIState == StateManager.AIState.Rage)
                {
                    enemyAI.health -= Mathf.RoundToInt(_weapon.Damage);

                    if (enemyAI.health <= 0)
                    {
                        Destroy(hit.collider.gameObject);
                    }
                }
            }

            var foodPile = hit.collider.GetComponent<FoodPile>();

            if (foodPile != null)
            {
                foodPile.Hit(1);
            }
        }

        attackCooldown = 1.0f / _weapon.AttackSpeed;
        isCoolingDown = true;
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


