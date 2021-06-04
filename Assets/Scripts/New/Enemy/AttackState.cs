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
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        if (currentAttack != null)
        {
            if (enemyManager.distanceFromTarget < currentAttack.distanceNeededToAttack.min)
            {
                return this;
            }
            else if (enemyManager.distanceFromTarget < currentAttack.distanceNeededToAttack.max)
            {
                if (enemyManager.viewableAngle <= currentAttack.attackAngle * 0.5f)
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

    private void GetNewAttack(EnemyManager enemyManager)
    {
        Vector3 targetsDirection = enemyManager.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
        enemyManager.distanceFromTarget = targetsDirection.magnitude;

        List<EnemyAttackAction> possibleAttacks = new List<EnemyAttackAction>(attacks.Length);
        int maxScore = 0;

        foreach (var attackAction in attacks)
        {
            if (attackAction.distanceNeededToAttack.IsInRange(enemyManager.distanceFromTarget))
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
