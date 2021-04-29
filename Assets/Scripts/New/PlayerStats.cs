using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    public Healthbar healthbar;

    private AnimatorHandler animHandler;

    void Awake()
    {
        animHandler = GetComponentInChildren<AnimatorHandler>();
    }

    protected override void Start()
    {
        base.Start();

        healthbar.SetHealth(currentHealth);
    }

    public override void TakeDamage(int damage)
    {
        if (currentHealth <= 0)
        {
            return;
        }
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        healthbar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animHandler.PlayTargetAnimation(AnimatorHandler.hashDeath1, true);
            //TODO: Handle player death
        }
        else
        {
            animHandler.PlayTargetAnimation(AnimatorHandler.hashDamage1, true);
        }
    }
}
