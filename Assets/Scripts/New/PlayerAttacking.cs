using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{
    private PlayerAnimatorHandler animatorHandler;
    private PlayerManager playerManager;
    private PlayerStats stats;
    private PlayerInventory inventory;
    private InputHandler inputHandler;
    private WeaponSlotManager weaponSlotManager;

    private string lastAttack;

    void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        stats = GetComponentInParent<PlayerStats>();
        inventory = GetComponentInParent<PlayerInventory>();
        inputHandler = GetComponentInParent<InputHandler>();

        animatorHandler = GetComponent<PlayerAnimatorHandler>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (inputHandler.comboFlag)
        {
            animatorHandler.SetAnimBool(AnimatorHandler.hashCanCombo, false);

            string attack = null;
            if (lastAttack == weapon.oneHandLightAttack1)
            {
                attack = weapon.oneHandLightAttack2;
            }
            else if (lastAttack == weapon.oneHandLightAttack2)
            {
                attack = weapon.oneHandLightAttack3;
            }
            else if (lastAttack == weapon.twoHandLightAttack1)
            {
                attack = weapon.twoHandLightAttack2;
            }

            if (attack == null)
            {
                throw new System.NullReferenceException();
            }
            animatorHandler.PlayTargetAnimation(attack, true);
            lastAttack = attack;
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        weaponSlotManager.attackingWeapon = weapon;
        string attack;

        if (inputHandler.twoHandWieldFlag)
        {
            attack = weapon.twoHandLightAttack1;
        }
        else
        {
            attack = weapon.oneHandLightAttack1;
        }
        animatorHandler.PlayTargetAnimation(attack, true);
        lastAttack = attack;
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        weaponSlotManager.attackingWeapon = weapon;
        string attack;

        if (inputHandler.twoHandWieldFlag)
        {
            //TODO: add two-handed-heavy-attack
            throw new System.NotImplementedException();
        }
        else
        {
            attack = weapon.oneHandHeavyAttack1;
        }
        animatorHandler.PlayTargetAnimation(attack, true);
        lastAttack = attack;
    }

    public void HandleRBAction()
    {
        switch (inventory.rightHandWeapon.weaponType)
        {
            case WeaponItem.WeaponType.Meele:
                PerformRBMeleeAction();
                break;
            case WeaponItem.WeaponType.Spell:
                PerformRBSpellAction(inventory.rightHandWeapon);
                break;
            default:
                throw new System.NotImplementedException();
        }

    }

    #region Attack Actions

    private void PerformRBMeleeAction()
    {
        if (playerManager.canCombo)
        {
            inputHandler.comboFlag = true;
            HandleWeaponCombo(inventory.rightHandWeapon);
            inputHandler.comboFlag = false;
        }
        else
        {
            if (playerManager.isInteracting || playerManager.canCombo)
            {
                return;
            }
            animatorHandler.anim.SetBool(AnimatorHandler.hashIsUsingRightHand, true);
            HandleLightAttack(inventory.rightHandWeapon);
        }
    }

    private void PerformRBSpellAction(WeaponItem weapon)
    {
        if (playerManager.isInteracting)
        {
            return;
        }
        if (weapon.weaponType == WeaponItem.WeaponType.Spell)
        {
            if (inventory.currentSpell != null && inventory.currentSpell.weaponType == WeaponItem.WeaponType.Spell)
            {
                if (stats.mana.currentValue >= inventory.currentSpell.manaCost)
                {
                    inventory.currentSpell.AttemptCastSpell(animatorHandler, stats);
                }
                else
                {
                    //SHRUG
                }
            }
        }
    }

    #endregion

    private void SuccessfullyCastSpell() /// Animator Event
    {
        inventory.currentSpell.SuccessfullyCastSpell(animatorHandler, stats);
    }
}
