using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    private PlayerManager playerManager;
    private Transform cameraObject;
    private InputHandler inputHandler;
    public Vector3 moveDirection;

    [HideInInspector]
    public Transform myTransform;
    [HideInInspector]
    public AnimatorHandler animHandler;
    [HideInInspector]
    public Rigidbody rigid;
    public GameObject normalCamera;

    [Header("Ground & Air Detection")]
    [SerializeField]
    private float groundDetectionRayStart = 0.5f;
    [SerializeField]
    private float minDistanceToBeginFall = 1f;
    [SerializeField]
    private float groundDirectionRayDistance = 0.2f;
    private LayerMask ignoreForGroundCheck;
    public float inAirTimer;


    [Header("Movement Stats")]
    [SerializeField]
    private float movementSpeed = 5f;
    [SerializeField]
    private float walkingSpeed = 3f;
    [SerializeField]
    private float sprintSpeed = 7.5f;
    [SerializeField]
    private float rotationSpeed = 10f;
    [SerializeField]
    private float fallingSpeed = 80;

    void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        rigid = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        animHandler = GetComponentInChildren<AnimatorHandler>();
        cameraObject = Camera.main.transform;
        myTransform = transform;
    }

    void Start()
    {
        animHandler.Initialize();

        playerManager.isGrounded = true;
        ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = playerManager != null && playerManager.isGrounded ? Color.blue : Color.red;
        Gizmos.DrawRay(transform.position + transform.up * groundDetectionRayStart, transform.up * -1f * groundDirectionRayDistance);
    }

    #region Movement
    private Vector3 normalVector = Vector3.zero;
    private Vector3 targetPosition;

    public void HandleMovement(float delta)
    {
        if (inputHandler.rollFlag || playerManager.isInteracting)
        {
            return;
        }

        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0f;

        float speed = movementSpeed;

        if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5f)
        {
            speed = sprintSpeed;
            playerManager.isSprinting = true;
            moveDirection *= speed;
        }
        else
        {
            if (inputHandler.moveAmount < 0.5f)
            {
                moveDirection *= walkingSpeed;
            }
            else
            {
                moveDirection *= speed;
            }
            playerManager.isSprinting = false;
        }

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigid.velocity = projectedVelocity;

        animHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0f, playerManager.isSprinting);

        if (animHandler.canRotate)
        {
            HandleRotation(delta);
        }
    }

    private void HandleRotation(float delta)
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverride = inputHandler.moveAmount;

        targetDir = cameraObject.forward * inputHandler.vertical;
        targetDir += cameraObject.right * inputHandler.horizontal;

        targetDir.Normalize();
        targetDir.y = 0f;

        if (targetDir == Vector3.zero)
        {
            targetDir = myTransform.forward;
        }

        Quaternion targetRot = Quaternion.LookRotation(targetDir);
        targetRot = Quaternion.Slerp(myTransform.rotation, targetRot, rotationSpeed * delta);

        myTransform.rotation = targetRot;
    }

    public void HandleRollingAndSprinting(float delta)
    {
        if (playerManager.isInteracting)
        {
            return;
        }

        if (inputHandler.rollFlag)
        {
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;

            if (inputHandler.moveAmount > 0f)
            {
                animHandler.PlayTargetAnimation(AnimatorHandler.hashRoll, true);

                moveDirection.y = 0f;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = rollRotation;
            }
            else
            {
                animHandler.PlayTargetAnimation(AnimatorHandler.hashBackstep, true);
            }
        }
    }

    public void HandleFalling(float delta, Vector3 moveDir)
    {
        playerManager.isGrounded = false;
        RaycastHit hit;
        Vector3 origin = transform.position;
        origin.y += groundDetectionRayStart;

        if (Physics.Raycast(origin, transform.forward, out hit, 0.4f))
        {
            moveDir = Vector3.zero;
        }

        if (playerManager.isAirborne)
        {
            rigid.AddForce(Vector3.down * fallingSpeed);
            rigid.AddForce(moveDir * fallingSpeed / 10f);
        }

        origin = origin + moveDir.normalized * groundDirectionRayDistance;

        targetPosition = transform.position;

        Debug.DrawRay(origin, Vector3.down * minDistanceToBeginFall, Color.red);
        if (Physics.Raycast(origin, Vector3.down, out hit, minDistanceToBeginFall, ignoreForGroundCheck))
        {
            normalVector = hit.normal;
            playerManager.isGrounded = true;
            targetPosition.y = hit.point.y;

            if (playerManager.isAirborne)
            {
                if (inAirTimer > 0.5f)
                {
                    Debug.Log("You were in the air for " + inAirTimer);
                    animHandler.PlayTargetAnimation(AnimatorHandler.hashLand, true);
                    inAirTimer = 0f;
                }
                else
                {
                    animHandler.PlayTargetAnimation(AnimatorHandler.hashLand, false);
                    //inAirTimer = 0f;
                }

                playerManager.isAirborne = false;
            }
        }
        else
        {
            if (playerManager.isGrounded)
            {
                playerManager.isGrounded = false;
            }

            if (!playerManager.isAirborne)
            {
                if (!playerManager.isInteracting)
                {
                    animHandler.PlayTargetAnimation(AnimatorHandler.hashFall, true);
                }

                rigid.velocity = rigid.velocity.normalized * (movementSpeed / 2f);
                playerManager.isAirborne = true;
            }
        }

        //if (playerManager.isGrounded)
        {
            if (playerManager.isInteracting || inputHandler.moveAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = targetPosition;
            }
        }
    }

    public void HandleJumping()
    {
        if (playerManager.isInteracting)
        {
            return;
        }

        if (inputHandler.jump_input)
        {
            if (inputHandler.moveAmount > 0)
            {
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;
                animHandler.PlayTargetAnimation(AnimatorHandler.hashJump, true);
                moveDirection.y = 0f;
                Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = jumpRotation;
            }
        }
    }
    #endregion
}
