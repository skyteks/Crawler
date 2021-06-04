using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private EnemyLocomotion locomotion;
    private EnemyAnimatorHandler animatorHandler;
    public bool isPerformingAction { get; private set; }

    public EnemyAttackAction[] attacks;
    private EnemyAttackAction currentAttack;

    [Header("AI Settings")]
    public float detectionRadius;
    public float detectionAngle = 50f;

    public float currentRecoveryTime = 0f;

    void Awake()
    {
        locomotion = GetComponent<EnemyLocomotion>();
        animatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
    }

    void Update()
    {
        HandleRecoveryTimer();
    }

    void FixedUpdate()
    {
        HandleCurrentAction();
    }

    private void HandleCurrentAction()
    {
        if (locomotion.currentTarget != null)
        {
            locomotion.distanceFromTarget = Vector3.Distance(locomotion.currentTarget.transform.position, transform.position);
        }

        if (locomotion.currentTarget == null)
        {
            locomotion.HandleDetection();
        }
        else if (locomotion.distanceFromTarget > locomotion.stoppingDistance)
        {
            locomotion.HandleMoveToTarget();
        }
        else if (locomotion.distanceFromTarget <= locomotion.stoppingDistance)
        {
            AttackTarget();
        }
    }

    private void HandleRecoveryTimer()
    {
        if (currentRecoveryTime > 0f)
        {
            currentRecoveryTime = Mathf.Max(0f, currentRecoveryTime - Time.deltaTime);
        }

        if (isPerformingAction)
        {
            if (currentRecoveryTime == 0f)
            {
                isPerformingAction = false;
            }
        }
    }

    #region Attacks

    private void AttackTarget()
    {
        if (isPerformingAction)
        {
            return;
        }

        if (currentAttack == null)
        {
            GetNewAttack();
        }
        else
        {
            isPerformingAction = true;
            currentRecoveryTime = currentAttack.recoveryTime;
            animatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
            currentAttack = null;
        }
    }

    private void GetNewAttack()
    {
        Vector3 targetsDirection = locomotion.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
        locomotion.distanceFromTarget = targetsDirection.magnitude;

        List<EnemyAttackAction> possibleAttacks = new List<EnemyAttackAction>(attacks.Length);
        int maxScore = 0;

        foreach (var attackAction in attacks)
        {
            if (attackAction.distanceNeededToAttack.IsInRange(locomotion.distanceFromTarget))
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

    #endregion
}
