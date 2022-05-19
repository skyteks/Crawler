using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    [System.Serializable]
    public struct PointsStat
    {
        public float max;
        [SerializeField, ReadOnly]
        private float current;
        public float currentValue => current;
        public float regenAmount;
        private float regenTimer;

        public void SetToMax()
        {
            current = max;
        }

        public void Change(float addition)
        {
            current = Mathf.Clamp(current + addition, 0f, max);
        }

        public void StartTimer()
        {
            regenTimer = 1;
        }

        public bool Regenerate()
        {
            if (regenTimer > 0)
            {
                return false;
            }

            regenTimer = Mathf.Clamp(regenTimer - Time.deltaTime, 0f, 1f);

            if (current < max && regenTimer == 0f)
            {
                current = Mathf.Clamp(current + regenAmount * Time.deltaTime, 0f, max);
                return true;
            }
            return false;
        }
    }

    public ABarUI healthbar;
    public ABarUI staminaBar;
    public ABarUI manaBar;
    public ABarUI expBar;

    private PlayerManager playerManager;
    private PlayerAnimatorHandler animHandler;

    public PointsStat stamina;
    public PointsStat mana;
    public int currentExp;

    protected override void Awake()
    {
        base.Awake();
        playerManager = GetComponent<PlayerManager>();
        animHandler = GetComponentInChildren<PlayerAnimatorHandler>();
    }

    protected override void Start()
    {
        base.Start();
        stamina.SetToMax();
        mana.SetToMax();

        healthbar?.SetFill(currentHealth, maxHealth);
        staminaBar?.SetFill(stamina.currentValue, stamina.max);
        manaBar?.SetFill(mana.currentValue, stamina.max);
    }

    public override void TakeDamage(int damage, bool noAnim = false)
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
            if (!noAnim)
            {
                animHandler.PlayTargetAnimation(AnimatorHandler.hashDeath1, true);
            }
        }
        else
        {
            if (!noAnim)
            {
                animHandler.PlayTargetAnimation(AnimatorHandler.hashDamage1, true);
            }
        }
    }

    public void Heal(int healing)
    {
        if (isDead)
        {
            return;
        }
        currentHealth = Mathf.Clamp(currentHealth + healing, 0, maxHealth);

        healthbar?.SetFill(currentHealth, maxHealth);
    }

    public void TakeStamina(int usage)
    {
        stamina.Change(-usage);

        staminaBar?.SetFill(stamina.currentValue, stamina.max);
    }

    public void TakeMana(int usage)
    {
        mana.Change(-usage);

        manaBar?.SetFill(mana.currentValue, mana.max);
    }

    public void RegenerateStamina()
    {
        if (playerManager.isInteracting)
        {
            stamina.StartTimer();
        }
        else if (stamina.Regenerate())
        {
            staminaBar.SetFill(stamina.currentValue, stamina.max);
        }
    }

    public void RegenerateMana()
    {
        if (playerManager.isInteracting)
        {
            mana.StartTimer();
        }
        else if (mana.Regenerate())
        {
            manaBar.SetFill(mana.currentValue, mana.max);
        }
    }

    public void AddExp(int exp)
    {
        currentExp += exp;
        expBar?.SetFill(currentExp, float.NaN);
    }
}
