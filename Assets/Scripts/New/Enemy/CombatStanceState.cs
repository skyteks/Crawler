using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : State
{
    public AttackState attackState;
    public PursueTargetState pursueTargetState;

    public override State Tick(EnemyManager enemyManager, EnemyStats stats, EnemyAnimatorHandler animatorHandler)
    {
        enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        if (enemyManager.currentRecoveryTime <= 0f && enemyManager.distanceFromTarget <= enemyManager.maxAttackingRange)
        {
            return attackState;
        }
        else if (enemyManager.distanceFromTarget > enemyManager.maxAttackingRange)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }
    }
}
