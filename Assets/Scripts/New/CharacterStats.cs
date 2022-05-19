using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStats : MonoBehaviour
{
    public int maxHealth;
    [SerializeField, ReadOnly]
    protected int currentHealth;
    public bool isDead => currentHealth <= 0;

    private Animator anim;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage, bool noAnim = false)
    {
        if (isDead)
        {
            return;
        }
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            if (!noAnim)
            {
                anim.Play(PlayerAnimatorHandler.hashDeath1);
            }
        }
        else
        {
            if (!noAnim)
            {
                anim.Play(PlayerAnimatorHandler.hashDamage1);
            }
        }
    }
}
