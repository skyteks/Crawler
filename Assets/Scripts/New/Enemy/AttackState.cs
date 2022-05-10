using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public CombatStanceState combatStanceState;

    public EnemyAttackAction[] attacks;
    private EnemyAttackAction currentAttack;

    public override State Tick(EnemyManager enemyManager, EnemyStats stats, EnemyAnimatorHandler animatorHandler)
    {
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
        float distanceFromTarget = targetDirection.magnitude;
        float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

        HandleRotateTowardsTarget(enemyManager);

        if (currentAttack != null)
        {
            if (distanceFromTarget < currentAttack.distanceNeededToAttack.min)
            {
                return this;
            }
            else if (distanceFromTarget < currentAttack.distanceNeededToAttack.max)
            {
                if (viewableAngle <= currentAttack.attackAngle * 0.5f)
                {
                    if (enemyManager.currentRecoveryTime <= 0f && !enemyManager.isPerformingAction)
                    {
                        animatorHandler.anim.SetFloat(AnimatorHandler.hashVertical, 0f, 0.1f, Time.deltaTime);
                        animatorHandler.anim.SetFloat(AnimatorHandler.hashHorizontal, 0f, 0.1f, Time.deltaTime);
                        animatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
                        enemyManager.isPerformingAction = true;
                        enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                        currentAttack = null;
                        return combatStanceState;
                    }
                }
            }
        }
        else
        {
            GetNewAttack(enemyManager);
        }
        return combatStanceState;
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

    private void GetNewAttack(EnemyManager enemyManager)
    {
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
        float distanceFromTarget = targetDirection.magnitude;
        float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

        List<EnemyAttackAction> possibleAttacks = new List<EnemyAttackAction>(attacks.Length);
        int maxScore = 0;

        foreach (var attackAction in attacks)
        {
            if (attackAction.distanceNeededToAttack.IsInRange(distanceFromTarget))
            {
                if (viewableAngle <= attackAction.attackAngle * 0.5f)
                {
                    maxScore += attackAction.attackScore;
                    possibleAttacks.Add(attackAction);
                }
            }
        }

        int random = Random.Range(0, maxScore);
        int tempScore = 0;

        foreach (var attackAction in possibleAttacks)
        {
            if (currentAttack != null)
            {
                return;
            }

            tempScore += attackAction.attackScore;

            if (tempScore > random)
            {
                currentAttack = attackAction;
            }
        }
    }
}
