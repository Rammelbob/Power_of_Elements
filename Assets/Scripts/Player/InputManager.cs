using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    PlayerInput playerInput;
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
    bool changeElement;

    public bool lightAttack;
    public bool heavyAttack;
    public bool speacialAttack;

    bool mouseUsed;
    bool gamePadUsed;
    bool changeInputType;
    GameObject currentMousePosition;

    Vector2[] pressedElement = { Vector2.up, Vector2.right, Vector2.down, Vector3.left };
    public bool elementalMovement;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerManager = GetComponent<PlayerManager>();
    }

    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = new PlayerInput();

            playerInput.CharacterControls.Move.started += OnMovementInput;
            playerInput.CharacterControls.Move.canceled += OnMovementInput;
            playerInput.CharacterControls.Move.performed += OnMovementInput;
            playerInput.CharacterControls.Run.performed += OnRun;
            playerInput.CharacterControls.Run.canceled += OnRun;
            playerInput.CharacterControls.ElemtalMovement.started += OnElementalMovement;
            playerInput.CharacterControls.ElemtalMovement.canceled += OnElementalMovement;
            playerInput.CharacterControls.Jump.started += OnJump;
            playerInput.CharacterControls.Dodge.started += OnDodge;
            playerInput.CharacterControls.ChangeElement.performed += OnChangeElement;
            
            playerInput.CharacterControls.LightAttack.performed += OnLightAttack;
            playerInput.CharacterControls.HeavyAttack.performed += OnHeavyAttack;
            playerInput.CharacterControls.SpecialAttack.performed += OnSpecialAttack;

            playerInput.CharacterControls.MoveCamera.canceled += OnMousMove;
            playerInput.CharacterControls.MoveCamera.performed += OnMousMove;

            playerInput.UI_Toggle.ToggleInventory.performed += OnToggleInventory;
            playerInput.UI_Toggle.ToggleInventory.canceled += OnToggleInventory;

            playerInput.UI.Point.performed += OnMouseInput;
            playerInput.UI.Navigate.performed += OnGamePadInput;

        }
        playerInput.Enable();
        playerInput.UI.Disable();
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
        changeElement = true;
    }

    void OnElementalMovement(InputAction.CallbackContext context)
    {
        elementalMovement = context.ReadValueAsButton();
    }

    void OnMousMove(InputAction.CallbackContext context)
    {
        lookMovement = context.ReadValue<Vector2>();
    }
    
    void OnToggleInventory(InputAction.CallbackContext context)
    {
        toggleInventory = context.ReadValueAsButton();
    } 
    
    void OnMouseInput(InputAction.CallbackContext context)
    {
        mouseUsed = true;
        if (gamePadUsed)
        {
            changeInputType = true;
            gamePadUsed = false;
        }
    } 
    
    void OnGamePadInput(InputAction.CallbackContext context)
    {
        gamePadUsed = true;
        if (mouseUsed)
        {
            changeInputType = true;
            mouseUsed = false;
        }
    }


    private void OnDisable()
    {
        playerInput.Disable();
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
        HandleUIInputs();
    }

    private void HandleCombatInput()
    {
        if (!playerManager.playerLocomotion.isGrounded)
            return;

        if (lightAttack)
        {
            //if (playerManager.playerLocomotion.isSprinting)
            //    playerManager.playerAttacker.HandleRunningAttack(playerManager.playerInventory.currentWeapon); 
            if (playerManager.canDoCombo)
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
                playerManager.playerAttacker.HandleLightAttack(playerManager.playerInventory.currentWeapon);
            }
        }

        if (heavyAttack)
        {
            //if (playerManager.playerLocomotion.isSprinting)
            //    playerManager.playerAttacker.HandleRunningAttack(playerManager.playerInventory.currentWeapon);
            if (playerManager.canDoCombo)
            {
                playerManager.playerAttacker.doWeaponCombo = true;
                playerManager.playerAttacker.comboLight = false;
            }
            else
            {
                if (playerManager.canDoCombo || playerManager.isInteracting)
                    return;
                playerManager.playerAttacker.HandleHeavyAttack(playerManager.playerInventory.currentWeapon);
            }
        }
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        playerManager.playerAnimatorManager.animator.SetFloat("inputX", movementInput.x, 0.05f, Time.deltaTime);
        playerManager.playerAnimatorManager.animator.SetFloat("inputY", movementInput.y, 0.05f, Time.deltaTime);
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

        if (changeElement)
        {
            changeElement = false;
            for (int i = 0; i < pressedElement.Length; i++)
            {
                if (pressedElement[i] == changeElementInput)
                {
                    playerManager.playerInventory.SetCurrentWeapon(i);
                    break;
                }
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

        if(Input.GetKeyDown(KeyCode.J))
        {
            playerManager.ui_Skillpoint_Handler.ToggleInventory(true);
            Cursor.lockState = CursorLockMode.None;
            playerInput.CharacterControls.Disable();
        }

        if (toggleInventory)
        {
            toggleInventory = false;
            if (playerManager.ui_Inventory_Handler.inventoryOpen)
            {
                playerManager.ui_Inventory_Handler.ToggleInventory(false);
                Cursor.lockState = CursorLockMode.Locked;
                playerInput.CharacterControls.Enable();
                playerInput.UI.Disable();
            }
            else
            {
                playerManager.ui_Inventory_Handler.ToggleInventory(true);
                Cursor.lockState = CursorLockMode.None;
                playerInput.CharacterControls.Disable();
                playerInput.UI.Enable();
            }
        }
    }

    private void HandleUIInputs()
    {
        if (mouseUsed)
        {
            if (changeInputType)
            {
                changeInputType = false;
                Cursor.visible = true;
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
        else if (gamePadUsed)
        {
            if (changeInputType)
            {
                changeInputType = false;
                Cursor.visible = false;
                EventSystem.current.SetSelectedGameObject(playerManager.ui_Inventory_Handler.itemParent.GetChild(0).gameObject);
                currentMousePosition = EventSystem.current.currentSelectedGameObject;
                Mouse.current.WarpCursorPosition(currentMousePosition.transform.position);
            }

            if (currentMousePosition != EventSystem.current.currentSelectedGameObject)
            {
                currentMousePosition = EventSystem.current.currentSelectedGameObject;
                Mouse.current.WarpCursorPosition(currentMousePosition.transform.position);
            }
        }
    }
}
