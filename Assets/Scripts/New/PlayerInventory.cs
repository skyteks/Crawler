using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private WeaponSlotManager weaponSlotManager;

    public WeaponItem rightHandWeapon;
    public WeaponItem leftHandWeapon;

    void Awake()
    {
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }

    void Start()
    {
        weaponSlotManager.LoadWeaponOnSlot(rightHandWeapon, WeaponHolderSlot.SlotTypes.rightHand);
        weaponSlotManager.LoadWeaponOnSlot(leftHandWeapon, WeaponHolderSlot.SlotTypes.leftHand);
    }
}
