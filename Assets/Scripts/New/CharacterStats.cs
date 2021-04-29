using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth;
    protected int currentHealth;

    private Animator anim;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        if (currentHealth <= 0)
        {
            return;
        }
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            anim.Play("Death 1");
        }
        else
        {
            anim.Play("Damage 1");
        }
    }
}
