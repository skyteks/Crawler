using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimatorHandler : AnimatorHandler
{
    private PlayerManager playerManager;
    private InputHandler inputHandler;
    private PlayerLocomotion locomotion;
    public bool canRotate;

    void OnAnimatorMove()
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

    public override void Initialize()
    {
        base.Initialize();
        playerManager = GetComponentInParent<PlayerManager>();
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

    public void PlayTargetAnimation(int targetAnimId, bool isInteracting)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool(hashIsInteracting, isInteracting);
        anim.CrossFade(targetAnimId, 0.2f);
    }

    public void SetAnimBool(string name, bool value)
    {
        anim.SetBool(name, value);
    }

    public void SetAnimBool(int id, bool value)
    {
        anim.SetBool(id, value);
    }

    public void ToggleCombo(int eventInfo) ///Animation Event
    {
        if (eventInfo != 0 && eventInfo != 1)
        {
            throw new System.ArgumentException();
        }
        bool toggle = eventInfo > 0;
        anim.SetBool(hashCanCombo, toggle);
    }

    public void ToggleIsInvulnerable(int eventInfo) ///Animation Event
    {
        if (eventInfo != 0 && eventInfo != 1)
        {
            throw new System.ArgumentException();
        }
        bool toggle = eventInfo > 0;
        anim.SetBool(AnimatorHandler.hashIsInvulnerable, toggle);
    }
}
