using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ConstantDamage : MonoBehaviour
{
    public int damagePerTick = 1;

    public float timeBetweenTicks = 1f;
    private float lastTick;

    void Awake()
    {
        Collider[] colliders = GetComponents<Collider>();
        bool trigger = false;
        foreach (Collider collider in colliders)
        {
            trigger = trigger || collider.isTrigger;
        }
        if (!trigger)
        {
            Debug.LogError("No Collider is set to isTrigger on Constant Damage", this);
        }
    }

    void OnTriggerStay(Collider other)
    {
        PlayerStats playerStats = other.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            if (lastTick + timeBetweenTicks < Time.time)
            {
                playerStats.TakeDamage(damagePerTick);
                lastTick = Time.time;
            }
        }
    }
}
