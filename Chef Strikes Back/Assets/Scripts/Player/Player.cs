using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Weapon _weapon;
    private float attackCooldown;
    private bool isCoolingDown;
    private int enemyKills;

    public float maxHealth;
    public float currentHealth;

    public float MaxRage;
    public float currentRage;
    [SerializeField]
    private Slider rageBar;

    private CharacterMovement character;

    public void Awake()
    {
        _weapon = new Weapon(0);
        character = GetComponent<CharacterMovement>();
    }

    private void Start()
    {
        maxHealth = 100;
        MaxRage = 100;
        currentRage = 0;
        currentHealth = maxHealth;
        rageBar.maxValue = MaxRage;
    }

    public void Update()
    {
        rageBar.value = currentRage;

        if (isCoolingDown)
        {
            attackCooldown -= Time.deltaTime;
            Debug.Log("In cooldown. Time remaining: " + Mathf.CeilToInt(attackCooldown));
            if (attackCooldown <= 0)
            {
                isCoolingDown = false;
                Debug.Log("Ready to attack!");
            }
        }
    }

    public void Attack(Vector2 mousePos)
    {
        if (isCoolingDown)
        {
            Debug.Log("Attack is in cooldown. Time remaining: " + Mathf.CeilToInt(attackCooldown) + " seconds.");
            return;
        }

        Collider2D hitCollider = Physics2D.OverlapPoint(mousePos);

        var direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
        character.FaceMovementDirection(direction);

        if (hitCollider != null)
        {
            var enemy = hitCollider.GetComponent<Enemy>();

            if (enemy)
            {
                if (Vector2.Distance(transform.position, hitCollider.transform.position) <= _weapon.Range)
                {
                    enemy.TakeDamage(Mathf.RoundToInt(_weapon.Damage));
                    Debug.Log("Hit " + hitCollider.name);
                }
                else
                {
                    Debug.Log("Range is not enough, missed!");
                }
            }
        }

        Debug.Log("Attacked with: " + _weapon.Name + ". Range: " + _weapon.Range + ", Damage: " + _weapon.Damage);

        attackCooldown = 2.0f / _weapon.AttackSpeed;
        isCoolingDown = true;
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
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (currentHealth >= 0)
        {
            Debug.Log("Player died.");
        }
    }
}


