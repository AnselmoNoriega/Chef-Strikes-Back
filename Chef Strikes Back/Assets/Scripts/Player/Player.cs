using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Player : MonoBehaviour
{

    private int enemyKills;
    private int money;

    public float attackCooldown;
    public float maxHealth;
    public float currentHealth;
    public float MaxRage;
    
    [SerializeField] SceneControl sceneControl;
    private Weapon _weapon;
    public float currentRage;
    [SerializeField]
    private Slider rageBar;
    [SerializeField]
    private Slider healthBar;

    public Animator animator;
    private CharacterMovement character;
    private SpriteRenderer spriteRenderer;

    public bool isCoolingDown;
    public bool attacking;
    public bool isDead;

    //states
    public InputAction move;
    public Rigidbody2D rb;
    private InputControls inputManager; 
    string[] attackDirections =
        {
        "Attack_Right", "Attack_RightTop", "Attack_Top", "Attack_LeftTop",
        "Attack_Left", "Attack_LeftBot", "Attack_Bot", "Attack_RightBot"
    };

    public void Awake()
    {
        inputManager = new InputControls();
        move = inputManager.Player.Move;
        _weapon = new Weapon(0);
        character = GetComponent<CharacterMovement>();
        animator = GetComponent<Animator>();
        //edited by kingston
        // 9.2
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attacking = false;
        isDead = false;
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

        if (isCoolingDown)
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0)
            {
                isCoolingDown = false;
            }
        }
    }

    public void Attack(Vector2 mousePos)
    {
        attacking = true;

        if (isCoolingDown)
        {
            return;
        }

        character.SetMoveDirection(Vector2.zero);
        character.SetCanMove(false);

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

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Die();
    }

    public void Die()
    {
        if (currentHealth <= 0)
        {
            isDead = true;
            sceneControl.switchToGameOverSence();
        }       
    }

    public void collectMoney(int amount)
    {
        money += amount;
    }

    
}


