using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{
    private AnimatorHandler animHandler;
    private InputHandler inputHandler;

    private string lastAttack;

    void Awake()
    {
        animHandler = GetComponentInChildren<AnimatorHandler>();
        inputHandler = GetComponent<InputHandler>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (inputHandler.comboFlag)
        {
            animHandler.SetAnimBool(AnimatorHandler.hashCanCombo, false);
            if (lastAttack == weapon.oneHandLightAttack1)
            {
                animHandler.PlayTargetAnimation(weapon.oneHandLightAttack2, true);
            }
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        string attack = weapon.oneHandLightAttack1;
        animHandler.PlayTargetAnimation(attack, true);
        lastAttack = attack;
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        string attack = weapon.oneHandHeavyAttack1;
        animHandler.PlayTargetAnimation(attack, true);
        lastAttack = attack;
    }
}
