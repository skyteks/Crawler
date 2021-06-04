using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorHandler : AnimatorHandler
{
    private EnemyManager enemyManager;

    void Awake()
    {
        Initialize();
        enemyManager = GetComponentInParent<EnemyManager>();
    }

    void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyManager.rigid.drag = 0f;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0f;
        Vector3 velocity = deltaPosition / delta;
        enemyManager.rigid.velocity = velocity;
    }
}
