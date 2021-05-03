using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private WeaponSlotManager weaponSlotManager;

    public WeaponItem rightHandWeapon;
    public WeaponItem leftHandWeapon;

    public WeaponItem unarmedWeapon;

    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[2];
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[2];

    public int currentRightWeaponIndex = 0;
    public int currentLeftWeaponIndex = 0;

    public List<WeaponItem> weaponsInventory = new List<WeaponItem>();

    void Awake()
    {
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }

    void Start()
    {
        rightHandWeapon = weaponsInRightHandSlots[0];
        leftHandWeapon = weaponsInLeftHandSlots[0];

        weaponSlotManager.LoadWeaponOnSlot(rightHandWeapon, WeaponHolderSlot.SlotTypes.rightHand);
        weaponSlotManager.LoadWeaponOnSlot(leftHandWeapon, WeaponHolderSlot.SlotTypes.leftHand);
    }

    public void ChangeRightHandWeapon()
    {
        currentRightWeaponIndex++;

        if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] != null)
        {
            rightHandWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], WeaponHolderSlot.SlotTypes.rightHand);
        }
        else if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] == null)
        {
            currentRightWeaponIndex++;
        }

        if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] != null)
        {
            rightHandWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], WeaponHolderSlot.SlotTypes.rightHand);
        }
        else if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] == null)
        {
            currentRightWeaponIndex++;
        }

        if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
        {
            currentRightWeaponIndex = -1;
            rightHandWeapon = unarmedWeapon;
            weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, WeaponHolderSlot.SlotTypes.rightHand);
        }
    }

    public void ChangeLeftHandWeapon()
    {
        currentLeftWeaponIndex++;

        if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] != null)
        {
            leftHandWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], WeaponHolderSlot.SlotTypes.leftHand);
        }
        else if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] == null)
        {
            currentLeftWeaponIndex++;
        }

        if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] != null)
        {
            leftHandWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], WeaponHolderSlot.SlotTypes.leftHand);
        }
        else if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] == null)
        {
            currentLeftWeaponIndex++;
        }

        if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
        {
            currentLeftWeaponIndex = -1;
            leftHandWeapon = unarmedWeapon;
            weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, WeaponHolderSlot.SlotTypes.leftHand);
        }
    }
}
