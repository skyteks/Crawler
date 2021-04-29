using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/WeaponItem")]
public class WeaponItem : Item
{
    public GameObject prefab;
    public bool isUnarmed;

    [Header("Idle Animations")]
    public string rightHandIdle;
    public string leftHandIdle;

    [Header("One Handed Attack Animations")]
    public string oneHandLightAttack1;
    public string oneHandLightAttack2;
    public string oneHandHeavyAttack1;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if ((name == null || name.Length == 0) && prefab != null)
        {
            name = prefab.name.Replace('_', ' ');
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif
}
