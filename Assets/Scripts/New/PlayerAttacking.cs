using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{
    private AnimatorHandler animHandler;

    private void Awake()
    {
        animHandler = GetComponentInChildren<AnimatorHandler>();
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        animHandler.PlayTargetAnimation(weapon.oneHandLightAttack1, true);
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        animHandler.PlayTargetAnimation(weapon.oneHandHeavyAttack1, true);
    }
}
