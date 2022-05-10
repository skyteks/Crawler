using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{
    private EnemyLocomotion locomotion;
    private EnemyAnimatorHandler animatorHandler;
    private EnemyStats stats;
    public NavMeshAgent agent { get; private set; }
    public Rigidbody rigid { get; private set; }

    public CharacterStats currentTarget;
    public State currentState;
    public bool isPerformingAction { get; set; }
    public bool isInteracting { get; set; }

    public float rotationSpeed = 15f;
    public float maxAttackingRange = 1.5f;

    [Header("AI Settings")]
    public float detectionRadius;
    public float detectionAngle = 50f;

    public float currentRecoveryTime = 0f;

    void Awake()
    {
        locomotion = GetComponent<EnemyLocomotion>();
        animatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
        stats = GetComponent<EnemyStats>();
        agent = GetComponentInChildren<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
    }

    void Start()
    {
        agent.enabled = false;
        rigid.isKinematic = false;
    }

    void Update()
    {
        HandleRecoveryTimer();

        isInteracting = animatorHandler.anim.GetBool("isInteracting");
    }

    void FixedUpdate()
    {
        HandleStateMachine();
    }

    private void HandleStateMachine()
    {
        if (currentState != null)
        {
            State nextState = currentState.Tick(this, stats, animatorHandler);

            if (nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

    private void SwitchToNextState(State state)
    {
        currentState = state;
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
}
