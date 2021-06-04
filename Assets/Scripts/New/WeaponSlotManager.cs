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
    private QuickSlotsUI quickSlotsUI;
    private PlayerStats playerStats;
    private InputHandler inputHandler;

    public WeaponItem attackingWeapon { private get; set; }

    void Awake()
    {
        anim = GetComponent<Animator>();
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
                    throw new System.NotImplementedException();
            }
        }
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem, WeaponHolderSlot.SlotTypes slotType)
    {
        switch (slotType)
        {
            case WeaponHolderSlot.SlotTypes.leftHand:
                leftHandSlot.currentWeaponItem = weaponItem;
                leftHandSlot.LoadWeaponPrefab(weaponItem);

                if (weaponItem != null)
                {
                    anim.CrossFade(weaponItem.leftHandIdle, 0.2f);
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
                    anim.CrossFade(weaponItem.twoHandedIdle, 0.2f);
                }
                else
                {
                    anim.CrossFade(PlayerAnimatorHandler.hashBothArmsEmpty, 0.2f);

                    carryOnBackSlot.UnloadWeapon(true);

                    if (weaponItem != null)
                    {
                        anim.CrossFade(weaponItem.rightHandIdle, 0.2f);
                    }
                    else
                    {
                        anim.CrossFade(PlayerAnimatorHandler.hashRightArmEmpty, 0.2f);
                    }
                }
                rightHandSlot.currentWeaponItem = weaponItem;
                rightHandSlot.LoadWeaponPrefab(weaponItem);
                break;
            case WeaponHolderSlot.SlotTypes.carryOnBack:

            default:
                break;
        }
        quickSlotsUI?.UpdateWeaponQuickslotsUI(slotType, weaponItem);

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
                break;
        }
    }

    public void ToggleLeftDamageCollider(int eventInfo)
    {
        bool toggle = eventInfo > 0;
        ToggleWeaponDamageCollider(WeaponHolderSlot.SlotTypes.leftHand, toggle);
    }

    public void ToggleRightDamageCollider(int eventInfo)
    {
        bool toggle = eventInfo > 0;
        ToggleWeaponDamageCollider(WeaponHolderSlot.SlotTypes.rightHand, toggle);
    }

    private void ToggleWeaponDamageCollider(WeaponHolderSlot.SlotTypes slotType, bool toggle)
    {
        switch (slotType)
        {
            case WeaponHolderSlot.SlotTypes.leftHand:
                leftHandDamageCollider.ToggleDamageCollider(toggle);
                break;
            case WeaponHolderSlot.SlotTypes.rightHand:
                rightHandDamageCollider.ToggleDamageCollider(toggle);
                break;
            default:
                break;

        }
    }

    #region Weapon StaminaDrainage
    public void DrainStaminaLightAttack()
    {
        playerStats.TakeStamina(attackingWeapon.staminaCostLightAttack);
    }

    public void DrainStaminaHeavyAttack()
    {
        playerStats.TakeStamina(attackingWeapon.staminaCostHeavyAttack);
    }
    #endregion
}
