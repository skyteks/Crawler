using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUISlot : InventoryUISlot
{
    public Image placeholderIcon;

    public WeaponHolderSlot.SlotTypes slotSide;
    public int slotIndex;

    void Awake()
    {
        uiManager = GetComponentInParent<UIManager>();
    }

    public override void AddItem(WeaponItem newItem)
    {
        item = newItem;
        if (item != null)
        {
            icon.sprite = item.icon;
        }
        icon.enabled = true;

        placeholderIcon.enabled = false;
    }

    public override void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;

        placeholderIcon.enabled = true;
    }

    public void SelectThisSlot()
    {
        uiManager.slotSide = slotSide;
        uiManager.slotIndex = slotIndex;
    }
}
