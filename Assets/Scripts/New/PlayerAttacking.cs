using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{
    private AnimatorHandler animHandler;
    private InputHandler inputHandler;
    private WeaponSlotManager weaponSlotManager;

    private string lastAttack;

    void Awake()
    {
        animHandler = GetComponentInChildren<AnimatorHandler>();
        inputHandler = GetComponent<InputHandler>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (inputHandler.comboFlag)
        {
            animHandler.SetAnimBool(AnimatorHandler.hashCanCombo, false);
            string attack = null;
            if (lastAttack == weapon.oneHandLightAttack1)
            {
                attack = weapon.oneHandLightAttack2;
            }
            else if (lastAttack == weapon.oneHandLightAttack2)
            {
                attack = weapon.oneHandLightAttack3;
            }

            if (attack == null)
            {
                throw new System.NullReferenceException();
            }
            animHandler.PlayTargetAnimation(attack, true);
            lastAttack = attack;
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        weaponSlotManager.attackingWeapon = weapon;

        string attack = weapon.oneHandLightAttack1;
        animHandler.PlayTargetAnimation(attack, true);
        lastAttack = attack;
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        weaponSlotManager.attackingWeapon = weapon;

        string attack = weapon.oneHandHeavyAttack1;
        animHandler.PlayTargetAnimation(attack, true);
        lastAttack = attack;
    }
}
