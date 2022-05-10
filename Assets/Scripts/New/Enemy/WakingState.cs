using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakingState : State
{
    public bool isSleeping;
    public float detectionRadius = 2f;
    public string sleepAnimation;
    public string wakeAnimation;
    public LayerMask detectionLayer;

    public PursueTargetState pursueTargetState;

    public override State Tick(EnemyManager enemyManager, EnemyStats stats, EnemyAnimatorHandler animatorHandler)
    {
        if (isSleeping && !enemyManager.isInteracting)
        {
            animatorHandler.PlayTargetAnimation(sleepAnimation, true);
        }

        #region Handle Target Detection

        Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if (characterStats != null)
            {
                Vector3 targetsDirection = characterStats.transform.position - enemyManager.transform.position;
                float viewableAngle = Vector3.Angle(targetsDirection, enemyManager.transform.forward);

                if (viewableAngle <= enemyManager.detectionAngle * 0.5f)
                {
                    enemyManager.currentTarget = characterStats;
                    isSleeping = false;
                    animatorHandler.PlayTargetAnimation(wakeAnimation, true);
                }
            }
        }

        #endregion

        #region Handloe State Change

        if (enemyManager.currentTarget != null)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }

        #endregion
    }

}
