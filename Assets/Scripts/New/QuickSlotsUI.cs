using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotsUI : MonoBehaviour
{
    public Image leftWeaponIcon;
    public Image rightWeaponIcon;

    public void UpdateWeaponQuickslotsUI(WeaponHolderSlot.SlotTypes slotType, WeaponItem weapon)
    {
        switch (slotType)
        {
            case WeaponHolderSlot.SlotTypes.leftHand:
                if (weapon.icon != null)
                {
                    leftWeaponIcon.sprite = weapon.icon;
                    leftWeaponIcon.enabled = true;
                }
                else
                {
                    leftWeaponIcon.sprite = null;
                    leftWeaponIcon.enabled = false;
                }
                break;
            case WeaponHolderSlot.SlotTypes.rightHand:
                if (weapon.icon != null)
                {
                    rightWeaponIcon.sprite = weapon.icon;
                    rightWeaponIcon.enabled = true;
                }
                else
                {
                    rightWeaponIcon.sprite = null;
                    rightWeaponIcon.enabled = false;
                }
                break;
        }
    }
}
