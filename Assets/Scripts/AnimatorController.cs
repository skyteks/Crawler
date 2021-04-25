using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class AnimatorController : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;

    private int hashVelocity = Animator.StringToHash("velocity");

    void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        anim.SetFloat(hashVelocity, agent.velocity.magnitude);
    }
}
