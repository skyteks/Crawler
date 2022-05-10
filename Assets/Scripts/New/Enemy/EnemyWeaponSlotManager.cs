using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : MonoBehaviour
{
    private WeaponHolderSlot leftHandSlot;
    private WeaponHolderSlot rightHandSlot;

    private DamageCollider leftHandDamageCollider;
    private DamageCollider rightHandDamageCollider;

    public WeaponItem leftHandWeapon;
    public WeaponItem rightHandWeapon;

    void Awake()
    {
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
                    continue;
                default:
                    throw new System.NotSupportedException();
            }
        }
    }

    void Start()
    {
        LoadWeaponsOnBothHands();
    }

    public void LoadWeaponOnSlot(WeaponItem weapon, WeaponHolderSlot.SlotTypes slotType)
    {
        switch (slotType)
        {
            case WeaponHolderSlot.SlotTypes.leftHand:
                leftHandSlot.currentWeaponItem = weapon;
                leftHandSlot.LoadWeaponPrefab(weapon);
                break;
            case WeaponHolderSlot.SlotTypes.rightHand:
                rightHandSlot.currentWeaponItem = weapon;
                rightHandSlot.LoadWeaponPrefab(weapon);
                break;
            default:
                throw new System.NotSupportedException();
        }

        LoadWeaponDamageCollider(slotType);
    }

    public void LoadWeaponsOnBothHands()
    {
        if (rightHandWeapon != null)
        {
            LoadWeaponOnSlot(rightHandWeapon, WeaponHolderSlot.SlotTypes.rightHand);
        }
        if (leftHandWeapon != null)
        {
            LoadWeaponOnSlot(leftHandWeapon, WeaponHolderSlot.SlotTypes.leftHand);
        }
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
        rightHandDamageCollider.ToggleDamageCollider(toggle);
    }


    #region Weapon StaminaDrainage
    public void DrainStaminaLightAttack() ///Animation Event
    {
    }

    public void DrainStaminaHeavyAttack() ///Animation Event
    {
    }
    #endregion
}
