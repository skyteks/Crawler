using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderSlot : MonoBehaviour
{
    public enum SlotTypes
    {
        none,
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
            currentWeaponInstance = null;
        }
    }

    public void LoadWeaponPrefab(WeaponItem weaponItem)
    {
        UnloadWeapon(true);
        if (weaponItem == null)
        {
            return;
        }

        GameObject instance = Instantiate(weaponItem.prefab) as GameObject;

        instance.transform.parent = parentOverride != null ? parentOverride : transform;
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;
        instance.transform.localScale = Vector3.one;

        currentWeaponInstance = instance;
    }
}
