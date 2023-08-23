using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iDamageable
{
    public bool isDead { get; }

    public int maxHealth { get; }
    public int health { get; }

    public void ApplyDamage(int damage);

    public void Heal(int amount);

    public void Die();
}
