using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerInput playerControls;
    PlayerManager playerManager;
    public Transform direction;
    public Transform target;

    public Vector2 movementInput;
    public Vector2 changeElementInput;
    Vector2 lookMovement;
    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    float mouseX, mouseY;
    [SerializeField] Vector2 lookSpeed = new Vector2(200, 170f);

    bool sprinting;
    bool startJump;
    bool startDodge;
    bool toggleInventory;

    public bool blockInput;
    public bool lightAttack;
    public bool heavyAttack;
    public bool speacialAttack;

    Vector2[] pressedElement = { Vector2.up, Vector2.right, Vector2.down, Vector3.left };
    public bool elementalMovement;
    int currentElementPressed;

    private void Awake()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        playerManager = GetComponent<PlayerManager>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerInput();

            playerControls.CharacterControls.Move.started += OnMovementInput;
            playerControls.CharacterControls.Move.canceled += OnMovementInput;
            playerControls.CharacterControls.Move.performed += OnMovementInput;
            playerControls.CharacterControls.Run.started += OnRun;
            playerControls.CharacterControls.Run.canceled += OnRun;
            playerControls.CharacterControls.ElemtalMovement.started += OnElementalMovement;
            playerControls.CharacterControls.ElemtalMovement.canceled += OnElementalMovement;
            playerControls.CharacterControls.Jump.started += OnJump;
            playerControls.CharacterControls.Dodge.started += OnDodge;
            playerControls.CharacterControls.ChangeElement.performed += OnChangeElement;
            
            playerControls.Combat.LightAttack.performed += OnLightAttack;
            playerControls.Combat.HeavyAttack.performed += OnHeavyAttack;
            playerControls.Combat.SpecialAttack.performed += OnSpecialAttack;

            playerControls.Combat.Block.performed += OnBlock;
            playerControls.Combat.Block.canceled += OnBlock;

            playerControls.UI.ToggleInventory.performed += OnInvetoryToggle;
            playerControls.UI.ToggleInventory.canceled += OnInvetoryToggle;

            playerControls.CameraControls.MoveCamera.canceled += OnMousMove;
            playerControls.CameraControls.MoveCamera.performed += OnMousMove;
        }
        playerControls.Enable();
    }

    void OnRun(InputAction.CallbackContext context)
    {
        sprinting = context.ReadValueAsButton();
    }

    void OnJump(InputAction.CallbackContext context)
    {
        startJump = context.ReadValueAsButton();
    }

    void OnDodge(InputAction.CallbackContext context)
    {
        startDodge = context.ReadValueAsButton();
    }

    void OnLightAttack(InputAction.CallbackContext context)
    {
        lightAttack = context.ReadValueAsButton();
    }

    void OnHeavyAttack(InputAction.CallbackContext context)
    {
        heavyAttack = context.ReadValueAsButton();
    }

    void OnSpecialAttack(InputAction.CallbackContext context)
    {
        speacialAttack = context.ReadValueAsButton();
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    void OnChangeElement(InputAction.CallbackContext context)
    {
        changeElementInput = context.ReadValue<Vector2>();
    }

    void OnElementalMovement(InputAction.CallbackContext context)
    {
        elementalMovement = context.ReadValueAsButton();
    }

    void OnBlock(InputAction.CallbackContext context)
    {
        blockInput = context.ReadValueAsButton();
    }

    void OnMousMove(InputAction.CallbackContext context)
    {
        lookMovement = context.ReadValue<Vector2>();
    }
    
    void OnInvetoryToggle(InputAction.CallbackContext context)
    {
        toggleInventory = context.ReadValueAsButton();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpInput();
        HandleDodgeInput();
        HandleCombatInput();
        HandleElementalChangeInput();
        HandleToggleInventory();
        HandleCameraInput();
    }

    private void HandleCombatInput()
    {
        if (lightAttack)
        {
            if (playerManager.playerLocomotion.isSprinting)
                playerManager.playerAttacker.HandleRunningAttack(playerManager.playerInventory.weapon); 
            else if (playerManager.canDoCombo)
            {
                playerManager.playerAttacker.doWeaponCombo = true;
                playerManager.playerAttacker.comboLight = true;
            }
            else
            {
                if (playerManager.isInteracting)
                    return;
                if (playerManager.canDoCombo)
                    return;
                playerManager.playerAttacker.HandleLightAttack(playerManager.playerInventory.weapon);
            }
        }

        if (heavyAttack)
        {
            if (playerManager.playerLocomotion.isSprinting)
                playerManager.playerAttacker.HandleRunningAttack(playerManager.playerInventory.weapon);
            else if (playerManager.canDoCombo)
            {
                playerManager.playerAttacker.doWeaponCombo = true;
                playerManager.playerAttacker.comboLight = false;
            }
            else
            {
                if (playerManager.canDoCombo || playerManager.isInteracting)
                    return;
                playerManager.playerAttacker.HandleHeavyAttack(playerManager.playerInventory.weapon);
            }
        }

        if (blockInput)
        {
            playerManager.playerAttacker.StartBlock();
        }
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        playerManager.playerAnimatorManager.animator.SetFloat("inputX", movementInput.x, 0.1f, Time.deltaTime);
        playerManager.playerAnimatorManager.animator.SetFloat("inputY", movementInput.y, 0.1f, Time.deltaTime);
        playerManager.playerAnimatorManager.animator.SetFloat("movementSpeed",moveAmount *(playerManager.playerLocomotion.isSprinting ? 2f : 1f), 0.1f, Time.deltaTime);
    }

    private void HandleSprintingInput()
    {
        if (sprinting && moveAmount > 0.5f)
        {
            playerManager.playerLocomotion.isSprinting = true;
        }
        else
        {
            playerManager.playerLocomotion.isSprinting = false;
        }
    }

    private void HandleJumpInput()
    {
        if (startJump)
        {
            startJump = false;
            playerManager.playerLocomotion.HandleJumping();
        }
    }

    private void HandleDodgeInput()
    {
        if (startDodge)
        {
            startDodge = false;
            playerManager.playerLocomotion.HandleDodgeing();
        }
    }

    private void HandleElementalChangeInput()
    {
        if (playerManager.isInteracting)
            return;

        for (int i = 0; i < pressedElement.Length; i++)
        {
            if (pressedElement[i] == changeElementInput)
            {
                if (i != currentElementPressed)
                {
                    playerManager.playerInventory.SetCurrentElement(i);
                    currentElementPressed = i;
                }
                break;
            }
        }
    }

    private void HandleCameraInput()
    {
        mouseX += lookMovement.x * Time.deltaTime * lookSpeed.x;
        mouseY -= lookMovement.y * Time.deltaTime * lookSpeed.y;

        mouseY = Mathf.Clamp(mouseY, -55, 75);

        target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        direction.rotation = Quaternion.Euler(0, mouseX, 0);
    }
    
    private void HandleToggleInventory()
    {
        if (toggleInventory)
        {
            toggleInventory = false;
            playerManager.ui_Inventory_Handler.ToggleInventory();
        }
    }
}
