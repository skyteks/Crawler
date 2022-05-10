using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : State
{
    public AttackState attackState;
    public PursueTargetState pursueTargetState;

    public override State Tick(EnemyManager enemyManager, EnemyStats stats, EnemyAnimatorHandler animatorHandler)
    {
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        HandleRotateTowardsTarget(enemyManager);

        if (enemyManager.isPerformingAction)
        {
            animatorHandler.anim.SetFloat(AnimatorHandler.hashVertical, 0f, 0.1f, Time.deltaTime);
        }

        if (enemyManager.currentRecoveryTime <= 0f && distanceFromTarget <= enemyManager.maxAttackingRange)
        {
            return attackState;
        }
        else if (distanceFromTarget > enemyManager.maxAttackingRange)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }
    }

    private void HandleRotateTowardsTarget(EnemyManager enemyManager)
    {
        if (enemyManager.isPerformingAction)
        {
            Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            direction.y = 0f;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = enemyManager.transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.agent.desiredVelocity);
            Vector3 targetVelocity = enemyManager.rigid.velocity;

            enemyManager.agent.enabled = true;
            enemyManager.agent.SetDestination(enemyManager.currentTarget.transform.position);
            enemyManager.rigid.velocity = targetVelocity;
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.agent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
    }
}
