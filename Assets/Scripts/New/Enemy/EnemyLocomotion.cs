using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocomotion : MonoBehaviour
{
    private EnemyManager enemyManager;
    private EnemyAnimatorHandler animatorHandler;
    private NavMeshAgent agent;
    public Rigidbody rigid { get; protected set; }

    public CharacterStats currentTarget;
    public LayerMask detectionLayer;

    public float distanceFromTarget { get; set; }
    public float stoppingDistance = 1f;
    public float rotationSpeed = 15f;


    void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        animatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
        agent = GetComponentInChildren<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
    }

    void Start()
    {
        agent.enabled = false;
        rigid.isKinematic = false;
    }

    public void HandleDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();
            if (characterStats != null)
            {
                Vector3 targetDirection = characterStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > -enemyManager.detectionAngle && viewableAngle < enemyManager.detectionAngle)
                {
                    currentTarget = characterStats;
                }
            }
        }
    }

    public void HandleMoveToTarget()
    {
        if (enemyManager.isPerformingAction)
        {
            return;
        }

        Vector3 targetDirection = currentTarget.transform.position - transform.position;
        distanceFromTarget = targetDirection.magnitude;
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        if (enemyManager.isPerformingAction)
        {
            animatorHandler.animator.SetFloat(AnimatorHandler.hashVertical, 0f, 0.1f, Time.deltaTime);
            enemyManager.enabled = false;
        }
        else
        {
            if (distanceFromTarget > stoppingDistance)
            {
                animatorHandler.animator.SetFloat(AnimatorHandler.hashVertical, 1f, 0.1f, Time.deltaTime);
            }
            else if (distanceFromTarget <= stoppingDistance)
            {
                animatorHandler.animator.SetFloat(AnimatorHandler.hashVertical, 0f, 0.1f, Time.deltaTime);

            }
        }

        HandleRotateTowardsTarget();
        agent.transform.localPosition = Vector3.zero;
        agent.transform.localRotation = Quaternion.identity;
    }

    private void HandleRotateTowardsTarget()
    {
        if (enemyManager.isPerformingAction)
        {
            Vector3 direction = currentTarget.transform.position - transform.position;
            direction.y = 0f;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed / Time.deltaTime);
        }
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(agent.desiredVelocity);
            Vector3 targetVelocity = rigid.velocity;

            agent.enabled = true;
            agent.SetDestination(currentTarget.transform.position);
            rigid.velocity = targetVelocity;
            transform.rotation = Quaternion.Slerp(transform.rotation, agent.transform.rotation, rotationSpeed / Time.deltaTime);
        }
    }
}
