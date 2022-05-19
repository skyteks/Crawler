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
    public static int hashIsUsingRightHand = Animator.StringToHash("isUsingRightHand");
    public static int hashIsUsingLeftHand = Animator.StringToHash("isUsingLeftHand");
    public static int hashIsInvulnerable = Animator.StringToHash("isInvulnerable");
    public static int hashIsDead = Animator.StringToHash("isDead");
    public static int hashCanRotate = Animator.StringToHash("canRotate");

    public static int hashRoll = Animator.StringToHash("Roll");
    public static int hashBackstep = Animator.StringToHash("Backstep");
    public static int hashLand = Animator.StringToHash("Land");
    public static int hashFall = Animator.StringToHash("Fall");
    public static int hashEmpty = Animator.StringToHash("Empty");
    public static int hashDeath1 = Animator.StringToHash("Death 1");
    public static int hashDamage1 = Animator.StringToHash("Damage 1");
    public static int hashPickUpItem = Animator.StringToHash("Pick Up Item");
    public static int hashJump = Animator.StringToHash("Jump");
    public static int hashDoBackStab = Animator.StringToHash("Back Stab");
    public static int hashGetBackStabbed = Animator.StringToHash("Back Stabbed");
    public static int hashOpenChest = Animator.StringToHash("Open Chest");

    public static int hashSleep = Animator.StringToHash("Sleep");
    public static int hashGetUp = Animator.StringToHash("GetUp");

    public static int hashRightArmEmpty = Animator.StringToHash("Right Arm Empty");
    public static int hashLeftArmEmpty = Animator.StringToHash("Left Arm Empty");
    public static int hashBothArmsEmpty = Animator.StringToHash("Both Arms Empty");

    public Animator anim { get; private set; }
    public bool canRotate;


    public virtual void Initialize()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool(hashCanRotate, false);
        anim.SetBool(hashIsInteracting, isInteracting);
        anim.CrossFade(targetAnim, 0.2f);
    }

    public void PlayTargetAnimation(int targetAnimHash, bool isInteracting)
    {
        anim.applyRootMotion = isInteracting;
        anim.SetBool(hashCanRotate, false);
        anim.SetBool(hashIsInteracting, isInteracting);
        anim.CrossFade(targetAnimHash, 0.2f);
    }

    public abstract void TakeSpecialDamage(); ///Animation Event

    public abstract void DropStuffOnDeath(); ///Animation Event
}
