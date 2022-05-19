using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimatorHandler : AnimatorHandler
{
    private EnemyManager enemyManager;
    private EnemyStats stats;

    void Awake()
    {
        Initialize();
        enemyManager = GetComponentInParent<EnemyManager>();
        stats = GetComponentInParent<EnemyStats>();
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
        anim.SetBool(hashCanCombo, toggle);
    }

    public void ToggleCanRotate(int eventInfo) ///Animation Event
    {
        if (eventInfo != 0 && eventInfo != 1)
        {
            throw new System.ArgumentException();
        }
        bool toggle = eventInfo > 0;
        anim.SetBool(hashCanRotate, toggle);
    }

    public override void TakeSpecialDamage() ///Animation Event
    {
        stats.TakeDamage(enemyManager.pendingSpecialAttackDamage, true);
        enemyManager.pendingSpecialAttackDamage = 0;
    }

    public override void DropStuffOnDeath() ///Animation Event
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.AddExp(stats.expDropped);
        }
    }
}
