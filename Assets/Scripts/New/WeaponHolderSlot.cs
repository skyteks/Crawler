using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderSlot : MonoBehaviour
{
    public enum SlotTypes
    {
        nothing,
        leftHand,
        rightHand,
    }

    public Transform parentOverride;
    public SlotTypes slotType;

    public GameObject currentWeaponInstance;

    public void UnloadWeapon(bool destroy = false)
    {
        if (currentWeaponInstance != null)
        {
            currentWeaponInstance.SetActive(false);
            if (destroy)
            {
                Destroy(currentWeaponInstance);
            }
        }
    }

    public void LoadWeaponPrefab(WeaponItem weaponItem)
    {
        if (weaponItem == null)
        {
            UnloadWeapon();
            return;
        }

        GameObject instance = Instantiate(weaponItem.prefab) as GameObject;

        if (instance != null)
        {
            instance.transform.parent = parentOverride != null ? parentOverride : transform;
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localScale = Vector3.one;
        }

        currentWeaponInstance = instance;
    }
}
