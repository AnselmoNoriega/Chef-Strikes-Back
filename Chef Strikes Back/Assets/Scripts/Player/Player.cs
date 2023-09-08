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
        Die();
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

        float spriteHeight = spriteRenderer.bounds.size.y;
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y + spriteHeight / 2);
        Vector2 attackDirection = (mousePos - (Vector2)transform.position).normalized;
        Debug.DrawRay(transform.position, attackDirection * _weapon.Range, Color.red, 1.0f);
        RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, attackDirection, _weapon.Range);
        string attackAnim = character.GetAttackDirection(attackDirection);
        animator.Play(attackAnim);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Player"))
            {
                continue;
            }

            var enemyAI = hit.collider.GetComponent<AI>();
            var enemyTest = hit.collider.CompareTag("Enemy");
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
            if (enemyTest)
            {
                Debug.Log("hited");
                Destroy(hit.collider.gameObject);
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



    public void InAttackingFinished()
    {
        animator.SetBool("IsAttacking", false);
        attacking = false;
        character.SetCanMove(true);
    }

    public void EnemyKilled()
    {
        enemyKills++;
        if (enemyKills % 1 == 0)
        {
            _weapon.UpgradeTier();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

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


