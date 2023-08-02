using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(menuName = "Player", fileName = "Player Stats")]
public class Player : ScriptableObject
{
    private Vector2 attackDirection;

    public int currentWeaponIndex = 0;
    public Weapon[] weapons;

    public int maxHealth = 100;
    public int currentHealth;

    public static Player Instance { get; private set; }

    private Transform playerTransform;

    private void Awake()
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
        weapons = new Weapon[]
        {
            new Knife(),
            new Colander(),
            new Spatula() 
        };

        playerTransform = this.GameObject().transform;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        SwitchWeapon(currentWeaponIndex);
    }

    public void SwitchWeapon(int weaponIndex)
    {
        currentWeaponIndex = weaponIndex;
        Debug.Log("Switched to weapon: " + weapons[currentWeaponIndex].Name);
    }

    public void SetAttackDirection(Vector2 direction)
    {
        attackDirection = direction;
    }

    public void Attack()
    {
        Weapon currentWeapon = weapons[currentWeaponIndex];
        int weaponDamage = currentWeapon.Damage;
        float weaponRange = currentWeapon.Range;

        int layerMask = 1 << LayerMask.NameToLayer("Player");
        layerMask = ~layerMask;

        RaycastHit2D hitInfo = Physics2D.Raycast(playerTransform.position,
            attackDirection,
            weaponRange,
            layerMask);

        Debug.DrawRay(playerTransform.position, attackDirection * weaponRange, Color.red, 2f);

        if (hitInfo)
        {
            var enemy = hitInfo.transform.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.TakeDamage(weaponDamage);
                Debug.Log("Hit " + hitInfo.transform.name);
            }
        }

        Debug.Log("Hit with:  " + currentWeapon.Name
                + ". Range: " + weaponRange
                + ", Damage: " + weaponDamage);
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


