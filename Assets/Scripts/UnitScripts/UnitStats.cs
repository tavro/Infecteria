using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats
{
    // FIELDS
    private int health;
    private int currentHealth;
    private int damage;
    private float attackSpeed;
    private float attackTimer;
    private float range;
    private bool aggressive;

    private int additionalHp = 0;

    public UnitStats(int hp, int currentHp, int dmg, float speed, float time, float r, bool state) {
        SetHealth(hp);
        SetCurrentHealth(currentHp);
        SetDamage(dmg);
        SetAttackSpeed(speed);
        SetAttackTimer(time);
        SetRange(r);
        SetAggressive(state);
    }

    // SETTERS
    public void SetDamage(int dmg) { damage = dmg; }
    public void SetHealth(int hp) { health = hp; }
    public void IncreaseAdditionalHealth(int amount) {
        additionalHp += amount;
        currentHealth += amount;
    }
    public void SetCurrentHealth(int hp) { currentHealth = hp; }
    public void DecreaseCurrentHealth(int amount) { currentHealth -= amount; }
    public void SetAttackSpeed(float speed) { attackSpeed = speed; }
    public void SetAttackTimer(float time) { attackTimer = time; }
    public void DecreaseAttackTimer(float amount) { attackTimer -= amount; }
    public void SetRange(float r) { range = r; }
    public void SetAggressive(bool state) { aggressive = state; }

    // GETTERS
    public int GetDamage() { return damage; }
    public int GetHealth() { return health + additionalHp; }
    public int GetCurrentHealth() { return currentHealth; }
    public float GetAttackSpeed() { return attackSpeed; }
    public float GetAttackTimer() { return attackTimer; }
    public float GetRange() { return range; }
    public bool IsAggressive() { return aggressive; }
}
