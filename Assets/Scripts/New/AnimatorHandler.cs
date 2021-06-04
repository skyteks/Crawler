using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class AnimatorHandler : MonoBehaviour
{
    public static int hashVertical = Animator.StringToHash("vertical");
    public static int hashHorizontal = Animator.StringToHash("horizontal");
    public static int hashIsInteracting = Animator.StringToHash("isInteracting");
    public static int hashCanCombo = Animator.StringToHash("canCombo");
    public static int hashIsAirborne = Animator.StringToHash("isAirborne");

    public static int hashRoll = Animator.StringToHash("Roll");
    public static int hashBackstep = Animator.StringToHash("Backstep");
    public static int hashLand = Animator.StringToHash("Land");
    public static int hashFall = Animator.StringToHash("Fall");
    public static int hashEmpty = Animator.StringToHash("Empty");
    public static int hashDeath1 = Animator.StringToHash("Death 1");
    public static int hashDamage1 = Animator.StringToHash("Damage 1");
    public static int hashPickUpItem = Animator.StringToHash("Pick Up Item");
    public static int hashJump = Animator.StringToHash("Jump");

    public static int hashRightArmEmpty = Animator.StringToHash("Right Arm Empty");
    public static int hashLeftArmEmpty = Animator.StringToHash("Left Arm Empty");
    public static int hashBothArmsEmpty = Animator.StringToHash("Both Arms Empty");

    protected Animator anim;
    public Animator animator { get { return anim; } }

    public virtual void Initialize()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool(hashIsInteracting, isInteracting);
        anim.CrossFade(targetAnim, 0.2f);
    }
}
