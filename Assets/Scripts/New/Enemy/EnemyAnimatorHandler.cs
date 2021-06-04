using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorHandler : AnimatorHandler
{
    private EnemyLocomotion locomotion;

    void Awake()
    {
        Initialize();
        locomotion = GetComponentInParent<EnemyLocomotion>();
    }

    void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        locomotion.rigid.drag = 0f;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0f;
        Vector3 velocity = deltaPosition / delta;
        locomotion.rigid.velocity = velocity;
    }
}
