using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 1;

    [SerializeField]
    [ReadOnly]
    private int currentHealth;

    public int current { get { return currentHealth; } }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        // reduce health by one life
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // destroy object
        currentHealth = 0;
        DropLoot lootDropping = GetComponent<DropLoot>();
        if (lootDropping != null)
        {
            lootDropping.Drop();
        }
        Destroy(gameObject);
    }
}
