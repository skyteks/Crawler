using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{
    public float backStabDistance = 1f;

    private PlayerAnimatorHandler animatorHandler;
    private PlayerManager playerManager;
    private PlayerStats stats;
    private PlayerInventory inventory;
    private InputHandler inputHandler;
    private WeaponSlotManager weaponSlotManager;

    private string lastAttack;
    private LayerMask backStabLayer;

    void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        stats = GetComponentInParent<PlayerStats>();
        inventory = GetComponentInParent<PlayerInventory>();
        inputHandler = GetComponentInParent<InputHandler>();

        animatorHandler = GetComponent<PlayerAnimatorHandler>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();

        backStabLayer = new LayerMask().ToNothing().Add(LayerMask.NameToLayer("BackStab"));
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (stats.stamina.currentValue <= 0f)
        {
            return;
        }

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
        if (stats.stamina.currentValue <= 0f)
        {
            return;
        }

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
        if (stats.stamina.currentValue <= 0f)
        {
            return;
        }

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

    public void PerformBackStabOrRiposte()
    {
        if (stats.stamina.currentValue <= 0f)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(inputHandler.specialAttackRayCastStartPoint.position, transform.forward, out hit, backStabDistance, backStabLayer))
        {

            CharacterManager enemyCharacterManager = hit.transform.GetComponentInParent<CharacterManager>();
            DamageCollider rightHandWeapon = weaponSlotManager.rightHandDamageCollider;

            if (enemyCharacterManager != null)
            {
                Debug.DrawRay(inputHandler.specialAttackRayCastStartPoint.position, transform.forward, Color.yellow, 1f);

                playerManager.transform.position = enemyCharacterManager.backStabTrigger.backStabberStandpoint.position;
                Vector3 rotationDirection = hit.transform.position - playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
                targetRotation = Quaternion.Slerp(playerManager.transform.rotation, targetRotation, Time.deltaTime * 100f);
                playerManager.transform.rotation = targetRotation;

                int specialDamage = inventory.rightHandWeapon.criticalDamageMultiplier * rightHandWeapon.currentWeaponDamage;
                enemyCharacterManager.pendingSpecialAttackDamage = specialDamage;

                animatorHandler.PlayTargetAnimation(AnimatorHandler.hashDoBackStab, true);
                enemyCharacterManager.GetComponentInChildren<EnemyAnimatorHandler>().PlayTargetAnimation(AnimatorHandler.hashGetBackStabbed, true);
            }
        }
    }

    #endregion

    private void SuccessfullyCastSpell() /// Animator Event
    {
        inventory.currentSpell.SuccessfullyCastSpell(animatorHandler, stats);
    }
}
