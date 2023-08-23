using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, iDamageable
{
    public bool isDead { get; private set; }

    [SerializeField] int _maxHealth;
    public int maxHealth => _maxHealth;

    public int health { get; private set; }

    float percent => (float)health / _maxHealth;

    [SerializeField] Component[] destroyComponentOnDie;

    [SerializeField] Image healthBar;

    [FoldoutGroup("Events"), SerializeField] UnityEvent OnDamageApplied = new UnityEvent();
    [FoldoutGroup("Events"), SerializeField] UnityEvent OnHealed = new UnityEvent();
    [FoldoutGroup("Events"), SerializeField] UnityEvent OnDie = new UnityEvent();

    private void Start()
    {
        health = _maxHealth;
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, _maxHealth);
        UpdateUI();
        OnDamageApplied.Invoke();

        if (health == 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, _maxHealth);
        UpdateUI();
        OnHealed.Invoke();
    }

    public void Die()
    {
        GetComponent<Animator>().SetBool("isDead", true);

        if(destroyComponentOnDie != null)
        {
            foreach(Component c in destroyComponentOnDie)
            {
                Destroy(c);
            }
        }

        OnDie.Invoke();
    }

    void UpdateUI()
    {
        healthBar.fillAmount = percent;
    }
}
