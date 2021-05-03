using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentWindowUI : MonoBehaviour
{
    public bool[] rightHandSlotsSelected = new bool[2];
    public bool[] leftHandSlotsSelected = new bool[2];

    private EquipmentUISlot[] weaponEquipmentUISlots;

    public void LoadWeaponsOnEquipmentScreen(PlayerInventory playerInventory)
    {
        if (weaponEquipmentUISlots == null)
        {
            weaponEquipmentUISlots = GetComponentsInChildren<EquipmentUISlot>(true);
        }

        for (int i = 0; i < weaponEquipmentUISlots.Length; i++)
        {
            int index = weaponEquipmentUISlots[i].slotIndex;
            switch (weaponEquipmentUISlots[i].slotSide)
            {
                case WeaponHolderSlot.SlotTypes.leftHand:
                    weaponEquipmentUISlots[i].AddItem(playerInventory.weaponsInLeftHandSlots[index]);
                    break;
                case WeaponHolderSlot.SlotTypes.rightHand:
                    weaponEquipmentUISlots[i].AddItem(playerInventory.weaponsInRightHandSlots[index]);
                    break;
            }
        }
    }

    public void SelectLeftHandSlot(int index)
    {
        leftHandSlotsSelected[index] = true;
    }

    public void SelectRightHandSlot(int index)
    {
        rightHandSlotsSelected[index] = true;
    }
}
