using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Vector2 attackDirection;
    private Weapon _weapon;
    private float attackCooldown;
    private bool isCoolingDown;
    private int enemyKills;

    public float maxHealth = 100;
    public float currentHealth;
    public static Player Instance { get; private set; }

    [SerializeField]private Transform playerTransform;

    public void Awake()
    {
        // create player
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.GameObject());
        }

        //create weapon
        _weapon = new Weapon(0);
        playerTransform = this.GameObject().transform;
    }
    public void Update()
    {
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
    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void Attack()
    {
        if (isCoolingDown)
        {
            Debug.Log("Attack is in cooldown. Time remaining: " + Mathf.CeilToInt(attackCooldown) + " seconds.");
            return;
        }

        Collider2D hitCollider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));

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


