using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLocomotion : MonoBehaviour
{
    private EnemyManager enemyManager;
    private EnemyAnimatorHandler animatorHandler;

    void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        animatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
    }
}
