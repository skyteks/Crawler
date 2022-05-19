using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStats : MonoBehaviour
{
    public int maxHealth;
    [SerializeField, ReadOnly]
    protected int currentHealth;
    public bool isDead => currentHealth <= 0;

    protected AnimatorHandler animatorHandler;

    protected virtual void Awake()
    {
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
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
            HandleDeath(noAnim);
        }
        else
        {
            if (!noAnim)
            {
                animatorHandler.PlayTargetAnimation(AnimatorHandler.hashDamage1, true);
            }
        }
    }

    protected virtual void HandleDeath(bool noAnim = false)
    {
        currentHealth = 0;
        if (!noAnim)
        {
            animatorHandler.PlayTargetAnimation(AnimatorHandler.hashDeath1, true);
        }
    }
}
