﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    public ABarUI healthbar;
    public ABarUI staminaBar;

    private PlayerManager playerManager;
    private PlayerAnimatorHandler animHandler;

    public float maxStamina;
    [SerializeField, ReadOnly]
    private float currentStamina;
    public float staminaRegenAmount = 1f;
    private float staminaRegenTimer;

    void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animHandler = GetComponentInChildren<PlayerAnimatorHandler>();
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
        if (playerManager.isInvulnerable)
        {
            return;
        }
        if (isDead)
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
        currentStamina = Mathf.Clamp(currentStamina - usage, 0f, maxStamina);

        staminaBar?.SetFill(currentStamina, maxStamina);
    }

    public void RegenerateStamina()
    {
        if (playerManager.isInteracting)
        {
            staminaRegenTimer = 1;
        }
        else
        {
            staminaRegenTimer = Mathf.Clamp(staminaRegenTimer - Time.deltaTime, 0f, 1f);

            if (currentStamina < maxStamina && staminaRegenTimer == 0f)
            {
                currentStamina = Mathf.Clamp(currentStamina + staminaRegenAmount * Time.deltaTime, 0f, maxStamina);
                staminaBar.SetFill(currentStamina, maxStamina);
            }
        }
    }
}
