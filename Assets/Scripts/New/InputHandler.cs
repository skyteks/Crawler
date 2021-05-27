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

    public bool rollFlag;
    public bool twoHandWieldFlag;
    public bool sprintFlag;
    public bool comboFlag;
    public bool inventoryFlag;
    public float rollInputTimer;

    private PlayerControls inputActions;
    private PlayerAttacking attacking;
    private PlayerInventory playerInventory;
    private PlayerManager playerManager;
    private WeaponSlotManager weaponSlotManager;
    private UIManager uiManager;

    private Vector2 movementInput;
    private Vector2 cameraInput;

    void Awake()
    {
        attacking = GetComponent<PlayerAttacking>();
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        uiManager = FindObjectOfType<UIManager>();
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
        }

        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    public void TickInput(float delta)
    {
        MoveInput(delta);
        HandleRollInput(delta);
        HandleAttackInput(delta);
        HandleQuickSlotInput();
        HandleInventoryInput();
        HandleTwoHandWieldInput();
    }

    private void MoveInput(float delta)
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
            if (playerManager.canCombo)
            {
                comboFlag = true;
                attacking.HandleWeaponCombo(playerInventory.rightHandWeapon);
                comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting || playerManager.canCombo)
                {
                    return;
                }
                attacking.HandleLightAttack(playerInventory.rightHandWeapon);
            }
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
