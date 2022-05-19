using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public Transform lockOnTransform;

    public BackStabTrigger backStabTrigger { get; protected set; }

    public int pendingSpecialAttackDamage;
}
