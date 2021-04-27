using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    private PlayerManager playerManager;
    private Transform cameraObject;
    private InputHandler inputHandler;
    Vector3 moveDirection;

    [HideInInspector]
    public Transform myTransform;
    [HideInInspector]
    public AnimatorHandler animHandler;
    [HideInInspector]
    public Rigidbody rigid;
    public GameObject normalCamera;

    [Header("Movement Stats")]
    [SerializeField]
    private float movementSpeed = 5f;
    [SerializeField]
    private float sprintSpeed = 7.5f;
    [SerializeField]
    private float rotationSpeed = 10f;

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
    }

    #region Movement
    private Vector3 normalVector = Vector3.zero;
    private Vector3 targetPosition;

    public void HandleMovement(float delta)
    {
        if (inputHandler.rollFlag)
        {
            return;
        }

        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0f;

        float speed = movementSpeed;

        if (inputHandler.sprintFlag)
        {
            speed = sprintSpeed;
            playerManager.isSprinting = true;
            moveDirection *= speed;
        }
        else
        {
            moveDirection *= speed;
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
        if (animHandler.isInteracting)
        {
            return;
        }

        if (inputHandler.rollFlag)
        {
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;

            if (inputHandler.moveAmount > 0f)
            {
                animHandler.PlayTargetAnimation("Roll", true);

                moveDirection.y = 0f;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = rollRotation;
            }
            else
            {
                animHandler.PlayTargetAnimation("Backstep", true);
            }
        }
    }
    #endregion
}
