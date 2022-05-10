using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    private WeaponHolderSlot leftHandSlot;
    private WeaponHolderSlot rightHandSlot;
    private WeaponHolderSlot carryOnBackSlot;

    private DamageCollider leftHandDamageCollider;
    private DamageCollider rightHandDamageCollider;

    private Animator anim;
    private PlayerManager playerManager;
    private QuickSlotsUI quickSlotsUI;
    private PlayerStats playerStats;
    private InputHandler inputHandler;

    public WeaponItem attackingWeapon { private get; set; }

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerManager = GetComponentInParent<PlayerManager>();
        quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
        playerStats = GetComponentInParent<PlayerStats>();
        inputHandler = GetComponentInParent<InputHandler>();

        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot slot in weaponHolderSlots)
        {
            switch (slot.slotType)
            {
                case WeaponHolderSlot.SlotTypes.leftHand:
                    leftHandSlot = slot;
                    break;
                case WeaponHolderSlot.SlotTypes.rightHand:
                    rightHandSlot = slot;
                    break;
                case WeaponHolderSlot.SlotTypes.carryOnBack:
                    carryOnBackSlot = slot;
                    break;
                default:
                    throw new System.NotSupportedException();
            }
        }
    }

    public void LoadWeaponOnSlot(WeaponItem weapon, WeaponHolderSlot.SlotTypes slotType)
    {
        switch (slotType)
        {
            case WeaponHolderSlot.SlotTypes.leftHand:
                leftHandSlot.currentWeaponItem = weapon;
                leftHandSlot.LoadWeaponPrefab(weapon);

                if (weapon != null)
                {
                    anim.CrossFade(weapon.leftHandIdle, 0.2f);
                }
                else
                {
                    anim.CrossFade(PlayerAnimatorHandler.hashLeftArmEmpty, 0.2f);
                }
                break;
            case WeaponHolderSlot.SlotTypes.rightHand:
                if (inputHandler.twoHandWieldFlag)
                {
                    carryOnBackSlot.LoadWeaponPrefab(leftHandSlot.currentWeaponItem);
                    leftHandSlot.UnloadWeapon(true);
                    anim.CrossFade(weapon.twoHandedIdle, 0.2f);
                }
                else
                {
                    anim.CrossFade(PlayerAnimatorHandler.hashBothArmsEmpty, 0.2f);

                    carryOnBackSlot.UnloadWeapon(true);

                    if (weapon != null)
                    {
                        anim.CrossFade(weapon.rightHandIdle, 0.2f);
                    }
                    else
                    {
                        anim.CrossFade(PlayerAnimatorHandler.hashRightArmEmpty, 0.2f);
                    }
                }
                rightHandSlot.currentWeaponItem = weapon;
                rightHandSlot.LoadWeaponPrefab(weapon);
                break;
            case WeaponHolderSlot.SlotTypes.carryOnBack:

            default:
                break;
        }
        quickSlotsUI?.UpdateWeaponQuickslotsUI(slotType, weapon);

        LoadWeaponDamageCollider(slotType);
    }

    private void LoadWeaponDamageCollider(WeaponHolderSlot.SlotTypes slotType)
    {
        switch (slotType)
        {
            case WeaponHolderSlot.SlotTypes.leftHand:
                leftHandDamageCollider = leftHandSlot.currentWeaponInstance.GetComponentInChildren<DamageCollider>();
                break;
            case WeaponHolderSlot.SlotTypes.rightHand:
                rightHandDamageCollider = rightHandSlot.currentWeaponInstance.GetComponentInChildren<DamageCollider>();
                break;
            default:
                throw new System.NotSupportedException();
        }
    }

    public void ToggleDamageCollider(int eventInfo) ///Animation Event
    {
        bool toggle = eventInfo > 0;
        if (toggle)
        {
            if (playerManager.isUsingLeftHand)
            {
                leftHandDamageCollider.ToggleDamageCollider(toggle);
            }
            else if (playerManager.isUsingRightHand)
            {
                rightHandDamageCollider.ToggleDamageCollider(toggle);
            }
        }
        else
        {
            leftHandDamageCollider.ToggleDamageCollider(toggle);
            rightHandDamageCollider.ToggleDamageCollider(toggle);

        }
    }

    #region Weapon StaminaDrainage
    public void DrainStaminaLightAttack() ///Animation Event
    {
        playerStats.TakeStamina(attackingWeapon.staminaCostLightAttack);
    }

    public void DrainStaminaHeavyAttack() ///Animation Event
    {
        playerStats.TakeStamina(attackingWeapon.staminaCostHeavyAttack);
    }
    #endregion
}
