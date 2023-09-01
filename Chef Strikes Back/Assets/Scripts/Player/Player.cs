using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Edit by kingston  -- 8/22
    [SerializeField] SceneControl sceneControl;
    //------------------------------------------
    private Weapon _weapon;
    public float attackCooldown;
    public bool isCoolingDown;
    private int enemyKills;

    public float maxHealth;
    public float currentHealth;

    public float MaxRage;
    public float currentRage;
    [SerializeField]
    private Slider rageBar;
    [SerializeField]
    private Slider healthBar;

    public Animator animator;
    private CharacterMovement character;

    public bool attacking;
    public bool isDead;
    private int money;

    public void Awake()
    {
        _weapon = new Weapon(0);
        character = GetComponent<CharacterMovement>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
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
            //Debug.Log("In cooldown. Time remaining: " + attackCooldown);
            if (attackCooldown <= 0)
            {
                isCoolingDown = false;
                //Debug.Log("Ready to attack!");
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

        Collider2D hitCollider = Physics2D.OverlapPoint(mousePos);
        var direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);

        string attackAnim = character.GetAttackDirection(direction);
        animator.Play(attackAnim);

        // Edited by Kingston     ---8//22
        // logic is Get the currentstate of Ai, if ai.state = rage, attackable.
        // and update AI's health in here, to destory the ai.
        if (hitCollider != null)
        {
            var enemyAI = hitCollider.GetComponent<AI>();

            if (enemyAI)
            {
                if (enemyAI.stateManager.CurrentAIState == StateManager.AIState.Rage)
                {
                    if (Vector2.Distance(transform.position, hitCollider.transform.position) <= _weapon.Range)
                    {
                        enemyAI.health -= Mathf.RoundToInt(_weapon.Damage);
                        //Debug.Log("Hit " + hitCollider.name);

                        if (enemyAI.health <= 0)
                        {
                            Destroy(hitCollider.gameObject);
                        }
                    }
                    else
                    {
                        //Debug.Log("Range is not enough, missed!");
                    }
                }
                else
                {
                    //Debug.Log("Can only attack AI in Rage state");
                }
            }

            var foodPile = hitCollider.GetComponent<FoodPile>();
            if(foodPile != null)
            {
                foodPile.Hit(1);
            }
            else
            {
                Debug.Log(foodPile);
            }
        }

        //Debug.Log("Attacked with: " + _weapon.Name + ". Range: " + _weapon.Range + ", Damage: " + _weapon.Damage);
        attackCooldown = 0.0f / _weapon.AttackSpeed;
        isCoolingDown = true;
    }

    public void InAttackingFinished()
    {
        //Debug.Log("Attack FINISHED");
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
        //Edit by kingston  -- 8/22
        currentHealth -= damageAmount;

    }

    public void Die()
    {
        //Edit by kingston  -- 8/22
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


