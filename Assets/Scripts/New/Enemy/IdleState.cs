using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public PursueTargetState pursueTargetState;
    public LayerMask detectionLayer;

    public override State Tick(EnemyManager enemyManager, EnemyStats stats, EnemyAnimatorHandler animatorHandler)
    {
        #region Handle Target detection
        Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, enemyManager.detectionRadius, detectionLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();
            if (characterStats != null)
            {
                Vector3 targetDirection = characterStats.transform.position - enemyManager.transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

                if (viewableAngle > -enemyManager.detectionAngle && viewableAngle < enemyManager.detectionAngle)
                {
                    enemyManager.currentTarget = characterStats;
                }
            }
        }
        #endregion

        if (enemyManager.currentTarget != null)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }
    }

    private void HandleDetection(EnemyManager enemyManager)
    {

    }
}
