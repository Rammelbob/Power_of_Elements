using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerInput playerControls;
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;
    CombatManager combatManager;
    PlayerAttacker playerAttacker;
    PlayerInventory playerInventory;
    PlayerManager playerManager;

    public Vector2 movementInput;
    public Vector2 changeElementInput;
    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool sprinting;
    public bool startJump;
    public bool startDodge;
    public bool comboFlag;

    public bool lightAttack;
    public bool heavyAttack;
    public bool speacialAttack;

    Vector2[] pressedElement = { Vector2.up, Vector2.right, Vector2.down, Vector3.left };
    public int currentElementpressed;
    public bool elementalMovement;

    public Elements[] playersElements = { Elements.Air, Elements.Fire, Elements.Electro, Elements.Water, Elements.Rock, Elements.Ice };
    public Elements currentElement;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        combatManager = GetComponent<CombatManager>();
        playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
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
        for (int i = 0; i < pressedElement.Length; i++)
        {
            if (pressedElement[i] == context.ReadValue<Vector2>())
            {
                currentElementpressed = i;
                currentElement = playersElements[i];
                Debug.Log(currentElement + " " + i);
                break;
            }
        }
    }

    void OnElementalMovement(InputAction.CallbackContext context)
    {
        elementalMovement = context.ReadValueAsButton();
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
    }

    private void HandleCombatInput()
    {
        if (lightAttack)
        {
            if (playerManager.canDoCombo)
            {
                comboFlag = true;
                playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                comboFlag = false;
            }
            else
            {
                if (playerManager.canDoCombo || playerManager.isInteracting)
                    return;
                playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
            }
        }

        if (heavyAttack)
        {
            if (playerManager.canDoCombo)
            {
                comboFlag = true;
                playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                comboFlag = false;
            }
            else
            {
                if (playerManager.canDoCombo || playerManager.isInteracting)
                    return;
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            } 
        }
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(playerLocomotion.movementSpeed, movementInput);
    }

    private void HandleSprintingInput()
    {
        if (sprinting && moveAmount > 0.5f && combatManager.CanUseStamina())
        {
            playerLocomotion.isSprinting = true;
        }
        else
        {
            playerLocomotion.isSprinting = false;
        }
    }

    private void HandleJumpInput()
    {
        if (startJump)
        {
            startJump = false;
            playerLocomotion.HandleJumping();
        }
    }

    private void HandleDodgeInput()
    {
        if (startDodge)
        {
            startDodge = false;
            playerLocomotion.HandleDodgeing();
        }
    }
}
