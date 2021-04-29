﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    private WeaponHolderSlot leftHandSlot;
    private WeaponHolderSlot rightHandSlot;

    private DamageCollider leftHandDamageCollider;
    private DamageCollider rightHandDamageCollider;

    private Animator anim;
    private QuickSlotsUI quickSlotsUI;

    void Awake()
    {
        anim = GetComponent<Animator>();
        quickSlotsUI = FindObjectOfType<QuickSlotsUI>();

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
                default:
                    break;
            }
        }
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem, WeaponHolderSlot.SlotTypes slotType)
    {
        switch (slotType)
        {
            case WeaponHolderSlot.SlotTypes.leftHand:
                leftHandSlot.LoadWeaponPrefab(weaponItem);

                if (weaponItem != null)
                {
                    anim.CrossFade(weaponItem.leftHandIdle, 0.2f);
                }
                else
                {
                    anim.CrossFade(AnimatorHandler.hashLeftArmEmpty, 0.2f);
                }
                break;
            case WeaponHolderSlot.SlotTypes.rightHand:
                rightHandSlot.LoadWeaponPrefab(weaponItem);

                if (weaponItem != null)
                {
                    anim.CrossFade(weaponItem.rightHandIdle, 0.2f);
                }
                else
                {
                    anim.CrossFade(AnimatorHandler.hashRightArmEmpty, 0.2f);
                }
                break;
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

    public void ToggleDamageCollider(string eventInfo)
    {
        //print("ToggleDamageCollider: " + eventInfo);
        if (eventInfo.Length != 2)
        {
            return;
        }
        WeaponHolderSlot.SlotTypes side;
        switch (eventInfo[0])
        {
            case 'L':
            case 'l':
                side = WeaponHolderSlot.SlotTypes.leftHand;
                break;
            case 'R':
            case 'r':
                side = WeaponHolderSlot.SlotTypes.rightHand;
                break;
            default:
                throw new System.ArgumentException();
        }
        bool toggle;
        switch (eventInfo[1])
        {
            case '0':
                toggle = false;
                break;
            case '1':
                toggle = true;
                break;
            default:
                throw new System.ArgumentException();
        }

        ToggleWeaponDamageCollider(side, toggle);
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
}