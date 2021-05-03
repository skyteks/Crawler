using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Windows")]
    public GameObject hudWindow;
    public GameObject selectWindow;
    public GameObject weaponInventoryWindow;
    public GameObject equipmentWindow;

    [Header("Equipment Slot Selected")]
    public WeaponHolderSlot.SlotTypes slotSide;
    public int slotIndex;

    [Header("Inventory")]
    private PlayerInventory playerInventory;
    public EquipmentWindowUI equipmentWindowUI;

    [Header("Weapons Inventory")]
    public GameObject inventorySlotPrefab;
    public Transform weaponsInventorySlotsParent;
    private InventoryUISlot[] weaponsInventoryUISlots;

    void Awake()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();

        weaponsInventoryUISlots = weaponsInventorySlotsParent.GetComponentsInChildren<InventoryUISlot>();
    }

    void Start()
    {
        equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
    }

    public void UpdateUI()
    {
        for (int i = 0; i < weaponsInventoryUISlots.Length; i++)
        {
            if (i < playerInventory.weaponsInventory.Count)
            {
                if (weaponsInventoryUISlots.Length < playerInventory.weaponsInventory.Count)
                {
                    Instantiate(inventorySlotPrefab, weaponsInventorySlotsParent);
                    weaponsInventoryUISlots = weaponsInventorySlotsParent.GetComponentsInChildren<InventoryUISlot>();
                }
                weaponsInventoryUISlots[i].AddItem(playerInventory.weaponsInventory[i]);
            }
            else
            {
                weaponsInventoryUISlots[i].ClearSlot();
            }
        }
    }

    public void ToggleSelectWindow(bool state)
    {
        selectWindow.SetActive(state);
    }

    public void CloseAllInventoryWindows()
    {
        ResetAllSelectedSlots();
        weaponInventoryWindow.SetActive(false);
        equipmentWindow.SetActive(false);
    }

    public void ResetAllSelectedSlots()
    {
        slotSide = WeaponHolderSlot.SlotTypes.none;
        slotIndex = -1;
    }
}
