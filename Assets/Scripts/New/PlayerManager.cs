using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    private InputHandler inputHandler;
    private Animator anim;
    private CameraHandler cameraHandler;
    private PlayerLocomotion locomotion;
    private PlayerStats stats;
    private PlayerAnimatorHandler animatorHandler;
    private InteractableUI interactableUI;
    public GameObject itemInteractableGO;

    [Header("Flags")]
    public bool isInteracting;
    public bool isSprinting;
    public bool isAirborne;
    public bool isGrounded;
    public bool canCombo;
    public bool isUsingLeftHand;
    public bool isUsingRightHand;
    public bool isInvulnerable;

    void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        locomotion = GetComponent<PlayerLocomotion>();
        stats = GetComponent<PlayerStats>();

        anim = GetComponentInChildren<Animator>();
        animatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        backStabTrigger = GetComponentInChildren<BackStabTrigger>();

        cameraHandler = FindObjectOfType<CameraHandler>();
        interactableUI = FindObjectOfType<InteractableUI>();
    }

    void Update()
    {
        isInteracting = anim.GetBool(AnimatorHandler.hashIsInteracting);
        canCombo = anim.GetBool(AnimatorHandler.hashCanCombo);
        isUsingLeftHand = anim.GetBool(AnimatorHandler.hashIsUsingLeftHand);
        isUsingRightHand = anim.GetBool(AnimatorHandler.hashIsUsingRightHand);
        isInvulnerable = anim.GetBool(AnimatorHandler.hashIsInvulnerable);
        animatorHandler.canRotate = anim.GetBool(AnimatorHandler.hashCanRotate);

        anim.SetBool(AnimatorHandler.hashIsAirborne, isAirborne);
        anim.SetBool(AnimatorHandler.hashIsDead, stats.isDead);

        float delta = Time.deltaTime;
        inputHandler.TickInput(delta);
        locomotion.HandleRollingAndSprinting(delta);
        locomotion.HandleJumping();
        stats.RegenerateStamina();
        stats.RegenerateMana();

        CheckForInteractableObject();
    }

    void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;
        locomotion.HandleMovement(delta);
        locomotion.HandleFalling(delta, locomotion.moveDirection);
        locomotion.HandleRotation(delta);
    }

    void LateUpdate()
    {
        inputHandler.rollFlag = false;
        inputHandler.rb_input = false;
        inputHandler.rt_input = false;
        inputHandler.d_pad_up = false;
        inputHandler.d_pad_down = false;
        inputHandler.d_pad_left = false;
        inputHandler.d_pad_right = false;
        inputHandler.a_input = false;
        inputHandler.jump_input = false;
        inputHandler.inventory_input = false;

        if (cameraHandler != null)
        {
            float delta = Time.deltaTime;
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, inputHandler.mouse);
        }

        if (isAirborne)
        {
            locomotion.inAirTimer = locomotion.inAirTimer + Time.deltaTime;
        }
    }

    #region Player Interactions

    public void CheckForInteractableObject()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position + Vector3.up, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
        {
            Interactable interactableObject = hit.collider.GetComponent<Interactable>();
            if (interactableObject != null)
            {
                interactableUI.interactableText.text = interactableObject.interactText;
                interactableUI.gameObject.SetActive(true);

                if (inputHandler.a_input)
                {
                    interactableObject.Interact(this);
                }
            }
        }
        else
        {
            interactableUI.gameObject.SetActive(false);

            if (itemInteractableGO != null && inputHandler.a_input)
            {
                itemInteractableGO.SetActive(false);
            }
        }
    }

    public void OpenChestInteraction(Transform playerStandpoint)
    {
        locomotion.rigid.velocity = Vector3.zero;
        transform.position = playerStandpoint.position;
        animatorHandler.PlayTargetAnimation(AnimatorHandler.hashOpenChest, true);
    }

    #endregion
}
