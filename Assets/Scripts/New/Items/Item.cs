using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public enum WeaponType
    {
        Meele,
        Spell,
    }
    [Header("Item Info")]
    public Sprite icon;

    public GameObject prefab;
    public bool isUnarmed;

    [Header("Weapon Info")]
    public WeaponType weaponType;

#if UNITY_EDITOR
    protected void OnValidate()
    {
        if ((name == null || name.Length == 0) && prefab != null)
        {
            name = prefab.name.Replace('_', ' ');
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif
}
