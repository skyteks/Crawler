using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public Vector2 mouse;

    public bool b_input;
    public bool a_input;
    public bool y_input;
    public bool rb_input;
    public bool rt_input;
    public bool jump_input;
    public bool d_pad_up;
    public bool d_pad_down;
    public bool d_pad_left;
    public bool d_pad_right;
    public bool inventory_input;
    public bool lockOn_input;
    public bool right_stick_left_input;
    public bool right_stick_right_input;

    public bool rollFlag;
    public bool twoHandWieldFlag;
    public bool sprintFlag;
    public bool comboFlag;
    public bool lockOnFlag;
    public bool inventoryFlag;
    public float rollInputTimer;

    private PlayerControls inputActions;
    private PlayerAttacking attacking;
    private PlayerInventory playerInventory;
    private PlayerManager playerManager;
    private CameraHandler cameraHandler;
    private WeaponSlotManager weaponSlotManager;
    private AnimatorHandler animatorHandler;
    private UIManager uiManager;

    private Vector2 movementInput;
    private Vector2 cameraInput;

    void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
        attacking = GetComponentInChildren<PlayerAttacking>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        uiManager = FindObjectOfType<UIManager>();
        cameraHandler = FindObjectOfType<CameraHandler>();
    }

    void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            inputActions.PlayerActions.RB.performed += i => rb_input = true;
            inputActions.PlayerActions.RT.performed += i => rt_input = true;
            inputActions.PlayerQuickslotActions.DPadRight.performed += i => d_pad_right = true;
            inputActions.PlayerQuickslotActions.DPadLeft.performed += i => d_pad_left = true;
            inputActions.PlayerActions.A.performed += i => a_input = true;
            inputActions.PlayerActions.Jump.performed += i => jump_input = true;
            inputActions.PlayerActions.Inventory.performed += i => inventory_input = true;
            inputActions.PlayerActions.Y.performed += i => y_input = true;
            inputActions.PlayerActions.LockOn.performed += i => lockOn_input = true;
            inputActions.PlayerMovement.SwitchLockOnLeft.performed += i => right_stick_left_input = true;
            inputActions.PlayerMovement.SwitchLockOnRight.performed += i => right_stick_right_input = true;
        }

        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    public void TickInput(float delta)
    {
        HandleMoveInput(delta);
        HandleRollInput(delta);
        HandleAttackInput(delta);
        HandleQuickSlotInput();
        HandleInventoryInput();
        HandleLockOnInput();
        HandleTwoHandWieldInput();
    }

    private void HandleMoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouse = cameraInput;
    }

    private void HandleRollInput(float delta)
    {
        b_input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;
        sprintFlag = b_input;

        if (b_input)
        {
            rollInputTimer += delta;
        }
        else
        {
            if (rollInputTimer > 0f && rollInputTimer < 0.5f)
            {
                sprintFlag = false;
                rollFlag = true;
            }

            rollInputTimer = 0f;
        }
    }

    private void HandleAttackInput(float delta)
    {
        if (rb_input)
        {
            attacking.HandleRBAction();
        }

        if (rt_input)
        {
            if (playerManager.isInteracting)
            {
                return;
            }
            attacking.HandleHeavyAttack(playerInventory.rightHandWeapon);
        }
    }

    private void HandleQuickSlotInput()
    {
        if (d_pad_right)
        {
            playerInventory.ChangeRightHandWeapon();
        }
        else if (d_pad_left)
        {
            playerInventory.ChangeLeftHandWeapon();
        }
    }

    private void HandleInventoryInput()
    {
        if (inventory_input)
        {
            inventoryFlag = !inventoryFlag;

            uiManager.ToggleSelectWindow(inventoryFlag);
            if (inventoryFlag)
            {
                uiManager.UpdateUI();
            }
            else
            {
                uiManager.CloseAllInventoryWindows();
            }
            uiManager.hudWindow.SetActive(!inventoryFlag);
        }
    }

    private void HandleLockOnInput()
    {
        if (lockOn_input)
        {
            if (!lockOnFlag)
            {
                lockOn_input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.nearestLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else
            {
                lockOn_input = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTargets();
            }
        }

        if (lockOnFlag)
        {
            if (right_stick_left_input)
            {
                right_stick_left_input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.leftLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockOnTarget;
                }
            }
            else if (right_stick_right_input)
            {
                right_stick_right_input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.rightLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockOnTarget;
                }
            }
        }

        cameraHandler.SetCameraHeight();
    }

    private void HandleTwoHandWieldInput()
    {
        if (y_input)
        {
            y_input = false;
            twoHandWieldFlag = !twoHandWieldFlag;

            if (twoHandWieldFlag)
            {
                weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightHandWeapon, WeaponHolderSlot.SlotTypes.rightHand);
            }
            else
            {
                weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightHandWeapon, WeaponHolderSlot.SlotTypes.rightHand);
                weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftHandWeapon, WeaponHolderSlot.SlotTypes.leftHand);
            }
        }
    }
}
