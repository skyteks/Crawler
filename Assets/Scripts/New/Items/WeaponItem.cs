using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/WeaponItem")]
public class WeaponItem : Item
{
    [Header("Stamina Costs")]
    public int staminaCostLightAttack;
    public int staminaCostHeavyAttack;

    [Header("Idle Animations")]
    public string rightHandIdle;
    public string leftHandIdle;
    public string twoHandedIdle;

    [Header("One Handed Attack Animations")]
    public string oneHandLightAttack1;
    public string oneHandLightAttack2;
    public string oneHandLightAttack3;
    [Space]
    public string oneHandHeavyAttack1;
    [Space]
    public string twoHandLightAttack1;
    public string twoHandLightAttack2;
}
