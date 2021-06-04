using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueTargetState : State
{
    public CombatStanceState combatStanceState;

    public override State Tick(EnemyManager enemyManager, EnemyStats stats, EnemyAnimatorHandler animatorHandler)
    {
        if (enemyManager.isPerformingAction)
        {
            return this;
        }

        Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
        enemyManager.distanceFromTarget = targetDirection.magnitude;
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        if (enemyManager.distanceFromTarget > enemyManager.maxAttackingRange)
        {
            animatorHandler.anim.SetFloat(AnimatorHandler.hashVertical, 1f, 0.1f, Time.deltaTime);
        }
        else if (enemyManager.distanceFromTarget <= enemyManager.maxAttackingRange)
        {
            animatorHandler.anim.SetFloat(AnimatorHandler.hashVertical, 0f, 0.1f, Time.deltaTime);
        }

        HandleRotateTowardsTarget(enemyManager);
        enemyManager.agent.transform.localPosition = Vector3.zero;
        enemyManager.agent.transform.localRotation = Quaternion.identity;

        if (enemyManager.distanceFromTarget <= enemyManager.maxAttackingRange)
        {
            return combatStanceState;
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
            enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, enemyManager.agent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
    }
}
