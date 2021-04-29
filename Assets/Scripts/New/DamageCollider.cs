using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageCollider : MonoBehaviour
{
    public int curWepDmg = 1;

    private Collider damageCollider;

    void Awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.isTrigger = true;
        damageCollider.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        print("hit " + other.gameObject);
        CharacterStats stats = other.GetComponent<CharacterStats>();
        if (stats != null)
        {
            stats.TakeDamage(curWepDmg);
        }
    }

    public void ToggleDamageCollider(bool toggle)
    {
        damageCollider.enabled = toggle;
    }
}
