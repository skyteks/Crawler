using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private InputHandler inputHandler;
    private Animator anim;
    private CameraHandler cameraHandler;
    private PlayerLocomotion locomotion;

    [Header("Flags")]
    public bool isInteracting;
    public bool isSprinting;
    public bool isAirborne;
    public bool isGrounded;

    private int hashIsInteracting = Animator.StringToHash("isInteracting");

    void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        anim = GetComponentInChildren<Animator>();
        locomotion = GetComponent<PlayerLocomotion>();
        cameraHandler = CameraHandler.instance;
    }

    void Update()
    {
        isInteracting = anim.GetBool(hashIsInteracting);

        float delta = Time.deltaTime;

        inputHandler.TickInput(delta);
        locomotion.HandleMovement(delta);
        locomotion.HandleRollingAndSprinting(delta);
        locomotion.HandleFalling(delta, locomotion.moveDirection);
    }

    void LateUpdate()
    {
        float delta = Time.deltaTime;

        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta * 10f);
            cameraHandler.HandleCameraRotation(delta * 10f, inputHandler.mouse);
        }

        inputHandler.rollFlag = false;
        inputHandler.sprintFlag = false;
        inputHandler.rb_input = false;
        inputHandler.rt_input = false;

        if (isAirborne)
        {
            locomotion.inAirTimer = locomotion.inAirTimer + Time.deltaTime;
        }
    }
}
