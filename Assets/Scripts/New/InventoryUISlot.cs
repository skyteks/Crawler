using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUISlot : MonoBehaviour
{
    private PlayerInventory playerInventory;
    private WeaponSlotManager weaponSlotManager;
    protected UIManager uiManager;

    public Image icon;
    protected WeaponItem item;

    void Awake()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        weaponSlotManager = FindObjectOfType<WeaponSlotManager>();
        uiManager = GetComponentInParent<UIManager>();
    }

    public virtual void AddItem(WeaponItem newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        gameObject.SetActive(true);
    }

    public virtual void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        gameObject.SetActive(false);
    }

    public void EquipThisItem()
    {
        switch (uiManager.slotSide)
        {
            case WeaponHolderSlot.SlotTypes.leftHand:
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlots[uiManager.slotIndex]);
                playerInventory.weaponsInLeftHandSlots[uiManager.slotIndex] = item;
                break;
            case WeaponHolderSlot.SlotTypes.rightHand:
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlots[uiManager.slotIndex]);
                playerInventory.weaponsInRightHandSlots[uiManager.slotIndex] = item;
                break;
            case WeaponHolderSlot.SlotTypes.none:
                return;
        }
        playerInventory.weaponsInventory.Remove(item);

        playerInventory.leftHandWeapon = playerInventory.weaponsInLeftHandSlots[playerInventory.currentLeftWeaponIndex];
        playerInventory.rightHandWeapon = playerInventory.weaponsInRightHandSlots[playerInventory.currentRightWeaponIndex];

        weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightHandWeapon, WeaponHolderSlot.SlotTypes.rightHand);
        weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftHandWeapon, WeaponHolderSlot.SlotTypes.leftHand);

        uiManager.equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
        uiManager.ResetAllSelectedSlots();
    }
}
