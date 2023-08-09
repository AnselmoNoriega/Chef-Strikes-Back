using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weapon
{
    private int tier;
    private float baseDamage = 10f;
    private float baseLength = 1.0f;
    private float baseRange = 1.0f;
    private float baseAttackSpeed = 1.0f;

    // Upgrades
    private float[] damageUpgrades = { 1.1f, 1.2f, 1.3f }; // +10%, +20%, +30%
    private float[] lengthUpgrades = { 1.05f, 1.1f, 1.2f }; // +5%, +10%, +20%
    private float[] rangeUpgrades = { 1.1f, 1.2f, 1.3f }; // +10%, +20%, +30%
    private float[] attackSpeedUpgrades = { 1.1f, 1.2f, 1.3f }; // +10%, +20%, +30%

    public string Name => "Knife";
    public int Tier => tier;
    public float Damage => baseDamage * damageUpgrades[tier];
    public float Length => baseLength * lengthUpgrades[tier];
    public float Range => baseRange * rangeUpgrades[tier];
    public float AttackSpeed => baseAttackSpeed * attackSpeedUpgrades[tier];

    public Weapon(int tier)
    {
        this.tier = tier >= 0 && tier < 3 ? tier : 0; // Make sure tier is within valid range
    }

    public void UpgradeTier()
    {
        tier = Math.Min(tier + 1, 2); // Maximum tier is 2
    }
}
