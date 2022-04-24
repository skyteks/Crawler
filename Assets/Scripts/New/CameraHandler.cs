using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private InputHandler inputHandler;
    private PlayerManager playerManager;
    public Transform targetTransform;
    public Transform cameraTransform;
    public Transform cameraPivotTransform;
    private Vector3 cameraTransformPosition;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    public LayerMask ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
    public LayerMask environmentLayers;

    public float lookSpeed = 0.1f;
    public float followSpeed = 0.1f;
    public float pivotSpeed = 0.03f;

    private float targetPosition;
    private float defaultPosition;
    private float lookAngle;
    private float pivotAngle;
    public float minPivot = -35f;
    public float maxPivot = 35f;

    public float cameraSphereRadius = 0.2f;
    public float cameraCollisionOffset = 0.2f;
    public float minCollisionOffset = 0.2f;
    public float freePivotPosition = 1.65f;
    public float lockedPivotPosition = 2.25f;

    public Transform currentLockOnTarget;
    private List<CharacterManager> availableTargets = new List<CharacterManager>();
    public Transform nearestLockOnTarget;
    public Transform leftLockOnTarget;
    public Transform rightLockOnTarget;
    public float maxLockOnDistance = 30f;

    void Awake()
    {
        defaultPosition = cameraTransform.localPosition.z;
        playerManager = FindObjectOfType<PlayerManager>();
        targetTransform = playerManager.transform;
        inputHandler = targetTransform.GetComponent<InputHandler>();
    }

    public void FollowTarget(float delta)
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
        transform.position = targetPosition;

        HandleCameraCollision(delta);
    }

    public void HandleCameraRotation(float delta, Vector2 mouse)
    {
        if (!inputHandler.lockOnFlag && currentLockOnTarget == null)
        {
            lookAngle += (mouse.x * lookSpeed) / delta;
            pivotAngle -= (mouse.y * pivotSpeed) / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            transform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = pivotAngle;

            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
        }
        else
        {
            float velocity = 0f;
            Vector3 direction = currentLockOnTarget.position - transform.position;
            direction.Normalize();
            direction.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;

            direction = currentLockOnTarget.position - cameraPivotTransform.position;
            direction.Normalize();

            targetRotation = Quaternion.LookRotation(direction);
            Vector3 eulerAngles = targetRotation.eulerAngles;
            eulerAngles.y = 0f;
            cameraPivotTransform.localEulerAngles = eulerAngles;
        }
    }

    private void HandleCameraCollision(float delta)
    {
        targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers, QueryTriggerInteraction.Ignore))
        {
            float distance = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetPosition = (distance - cameraCollisionOffset) * -1f;
        }

        if (Mathf.Abs(targetPosition) < -minCollisionOffset)
        {
            targetPosition = -minCollisionOffset;
        }

        cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
        cameraTransform.localPosition = cameraTransformPosition;
    }

    public void HandleLockOn()
    {
        float shortestDistance = float.PositiveInfinity;
        float shortestDistanceToLeftTarget = float.PositiveInfinity;
        float shortestDistanceToRightTarget = float.PositiveInfinity;

        Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 26);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager character = colliders[i].GetComponent<CharacterManager>();

            if (character != null)
            {
                Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                float distanceFromTarget = lockTargetDirection.magnitude;
                float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);

                if (character.transform.root != targetTransform.transform.root
                    && viewableAngle > -50f && viewableAngle < 50
                    && distanceFromTarget <= maxLockOnDistance)
                {
                    RaycastHit hit;
                    if (Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
                    {
                        Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position, Color.yellow);

                        if (environmentLayers.Includes(hit.transform.gameObject.layer))
                        {
                            availableTargets.Add(character);
                        }
                    }
                }
            }
        }

        for (int i = 0; i < availableTargets.Count; i++)
        {
            float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[i].transform.position);


            if (distanceFromTarget < shortestDistance)
            {
                shortestDistance = distanceFromTarget;
                nearestLockOnTarget = availableTargets[i].lockOnTransform;
            }

            if (inputHandler.lockOnFlag)
            {
                Vector3 relativeTargetPosition = currentLockOnTarget.InverseTransformPoint(availableTargets[i].transform.position);
                float distanceFromLeftTarget = currentLockOnTarget.transform.position.x - availableTargets[i].transform.position.x;
                float distanceFromRightTarget = currentLockOnTarget.transform.position.x + availableTargets[i].transform.position.x;

                if (relativeTargetPosition.x > 0f && distanceFromLeftTarget < shortestDistanceToLeftTarget)
                {
                    shortestDistanceToLeftTarget = distanceFromLeftTarget;
                    leftLockOnTarget = availableTargets[i].lockOnTransform;
                }

                if (relativeTargetPosition.x < 0f && distanceFromRightTarget < shortestDistanceToRightTarget)
                {
                    shortestDistanceToRightTarget = distanceFromRightTarget;
                    rightLockOnTarget = availableTargets[i].lockOnTransform;
                }
            }
        }
    }

    public void ClearLockOnTargets()
    {
        availableTargets.Clear();
        currentLockOnTarget = null;
        nearestLockOnTarget = null;
    }

    public void SetCameraHeight()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 newLockedPosition = Vector3.up * lockedPivotPosition;
        Vector3 newFreePosition = Vector3.up * freePivotPosition;

        if (currentLockOnTarget != null)
        {
            cameraPivotTransform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
        }
        else
        {
            cameraPivotTransform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.localPosition, newFreePosition, ref velocity, Time.deltaTime);
        }
    }
}
