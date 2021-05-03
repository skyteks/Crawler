using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    public ABarUI healthbar;
    public ABarUI staminaBar;

    private AnimatorHandler animHandler;

    public int maxStamina;
    [SerializeField, ReadOnly]
    private int currentStamina;

    void Awake()
    {
        animHandler = GetComponentInChildren<AnimatorHandler>();
    }

    protected override void Start()
    {
        base.Start();
        currentStamina = maxStamina;

        healthbar?.SetFill(currentHealth, maxHealth);
        staminaBar?.SetFill(currentStamina, maxStamina);
    }

    public override void TakeDamage(int damage)
    {
        if (currentHealth <= 0)
        {
            return;
        }
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        healthbar?.SetFill(currentHealth, maxHealth);

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

    public void TakeStamina(int usage)
    {
        currentStamina = Mathf.Clamp(currentStamina - usage, 0, maxStamina);

        staminaBar?.SetFill(currentStamina, maxStamina);
    }
}
