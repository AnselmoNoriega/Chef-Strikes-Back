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

    private int enemyKills;
    private int money;

    public float attackCooldown;
    public float maxHealth;
    public float currentHealth;
    public float MaxRage;
    
    [SerializeField] 
    SceneControl sceneControl;
    public Weapon _weapon;
    public float currentRage;
    [SerializeField]
    private Slider rageBar;
    [SerializeField]
    private Slider healthBar;

    public Animator animator;
    private CharacterMovement character;

    public bool isCoolingDown;

    //states
    public InputAction move;
    public Rigidbody2D rb;
    private InputControls inputManager;
    public PlayerStates playerState;
    private StateMachine<Player> stateMachine;
    string[] attackDirections =
        {
        "Attack_Right", "Attack_RightTop", "Attack_Top", "Attack_LeftTop",
        "Attack_Left", "Attack_LeftBot", "Attack_Bot", "Attack_RightBot"
    };

    public void Awake()
    {
        stateMachine = new StateMachine<Player>(this);
        inputManager = new InputControls();
        _weapon = new Weapon(0);
        inputManager = new InputControls();
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
        character = GetComponent<CharacterMovement>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        maxHealth = 100;
        MaxRage = 100;
        currentRage = 0;
        currentHealth = maxHealth;
        rageBar.maxValue = MaxRage;
    }

    public void Update()
    {
        rageBar.value = currentRage;
        healthBar.value = currentHealth / maxHealth;

        if(move.ReadValue<Vector2>() != Vector2.zero && playerState != PlayerStates.Attacking)
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

        stateMachine.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if(currentHealth <= 0)
        {
            sceneControl.switchToGameOverSence();
        }
    }

    public void collectMoney(int amount)
    {
        money += amount;
    }

    public void ChangeState(PlayerStates state)
    {
        if(playerState != state)
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
    }
}


