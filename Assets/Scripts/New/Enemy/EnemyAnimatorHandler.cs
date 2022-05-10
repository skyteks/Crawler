using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
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

    public void ToggleCombo(int eventInfo) ///Animation Event
    {
        if (eventInfo != 0 && eventInfo != 1)
        {
            throw new System.ArgumentException();
        }
        bool toggle = eventInfo > 0;
        //anim.SetBool(hashCanCombo, toggle);
    }
}
