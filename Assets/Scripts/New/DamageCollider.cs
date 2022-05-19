using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageCollider : MonoBehaviour
{
    public int currentWeaponDamage;

    private Collider damageCollider;

    void Awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.isTrigger = true;
        damageCollider.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        CharacterStats stats = other.GetComponent<CharacterStats>();
        if (stats != null)
        {
            stats.TakeDamage(currentWeaponDamage);
        }
    }

    public void ToggleDamageCollider(bool toggle)
    {
        damageCollider.enabled = toggle;
    }
}
