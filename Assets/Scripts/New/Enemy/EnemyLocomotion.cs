using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLocomotion : MonoBehaviour
{
    private EnemyManager enemyManager;
    private EnemyAnimatorHandler animatorHandler;

    public CapsuleCollider characterCollider;
    public CapsuleCollider characterCollisionBlockerCollider;

    void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        animatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
    }

    void Start()
    {
        Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
    }
}
