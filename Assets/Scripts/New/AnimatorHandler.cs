using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorHandler : MonoBehaviour
{
    private PlayerManager playerManager;
    private Animator anim;
    private InputHandler inputHandler;
    private PlayerLocomotion locomotion;
    private int hashVertical = Animator.StringToHash("vertical");
    private int hashHorizontal = Animator.StringToHash("horizontal");
    private int hashIsInteracting = Animator.StringToHash("isInteracting");
    public bool canRotate;



    public bool isInteracting
    {
        get
        {
            return anim.GetBool(hashIsInteracting);
        }
    }

    public void Initialize()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        anim = GetComponent<Animator>();
        inputHandler = GetComponentInParent<InputHandler>();
        locomotion = GetComponentInParent<PlayerLocomotion>();
    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
    {
        float vertical = Movement(verticalMovement);
        float horizontal = Movement(horizontalMovement);

        if (isSprinting)
        {
            vertical = 2f;
            horizontal = horizontalMovement;
        }

        anim.SetFloat(hashVertical, vertical, 0.1f, Time.deltaTime);
        anim.SetFloat(hashHorizontal, horizontal, 0.1f, Time.deltaTime);
    }

    private float Movement(float movement)
    {
        float value = 0f;

        if (movement > 0f && movement < 0.55f)
        {
            value = 0.5f;
        }
        else if (movement > 0.55f)
        {
            value = 1f;
        }
        else if (movement < 0f && movement > -0.55f)
        {
            value = -0.5f;
        }
        else if (movement < -0.55f)
        {
            value = -1f;
        }
        else
        {
            value = 0;
        }
        return value;
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool(hashIsInteracting, isInteracting);
        anim.CrossFade(targetAnim, 0.2f);
    }

    private void OnAnimatorMove()
    {
        if (!playerManager.isInteracting)
        {
            return;
        }

        float delta = Time.deltaTime;
        locomotion.rigid.drag = 0f;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0f;
        Vector3 velocity = deltaPosition / delta;
        locomotion.rigid.velocity = velocity;
    }
}
