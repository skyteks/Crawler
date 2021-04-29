using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorHandler : MonoBehaviour
{
    public static int hashVertical = Animator.StringToHash("vertical");
    public static int hashHorizontal = Animator.StringToHash("horizontal");
    public static int hashIsInteracting = Animator.StringToHash("isInteracting");
    public static int hashCanCombo = Animator.StringToHash("canCombo");

    public static int hashRoll = Animator.StringToHash("Roll");
    public static int hashBackstep = Animator.StringToHash("Backstep");
    public static int hashLand = Animator.StringToHash("Land");
    public static int hashFall = Animator.StringToHash("Fall");
    public static int hashEmpty = Animator.StringToHash("Empty");
    public static int hashDeath1 = Animator.StringToHash("Death 1");
    public static int hashDamage1 = Animator.StringToHash("Damage 1");

    public static int hashRightArmEmpty = Animator.StringToHash("Right Arm Empty");
    public static int hashLeftArmEmpty = Animator.StringToHash("Left Arm Empty");

    private PlayerManager playerManager;
    private Animator anim;
    private InputHandler inputHandler;
    private PlayerLocomotion locomotion;
    public bool canRotate;

    public bool animIsInteracting { get { return anim.GetBool(hashIsInteracting); } }

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

    public void ToggleCombo(int value)
    {
        if (value != 0 && value != 1)
        {
            throw new System.ArgumentException();
        }
        bool toggle = value == 1;
        anim.SetBool(hashCanCombo, toggle);
    }
}
