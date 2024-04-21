using System;
using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    protected float health;

    protected virtual void Awake()
    {
        health = maxHealth;
    }

    public virtual void Damage()
    {
        Damage(0.5f);
    }
    
    public virtual void Damage(float amount)
    {
        health -= amount;
    }
    
    public virtual void Heal()
    {
        health += 0.5f;
    }
    
    public virtual void Heal(float amount)
    {
        health += amount;
    }

    public bool IsDead()
    {
        return health <= 0f;
    }
}
