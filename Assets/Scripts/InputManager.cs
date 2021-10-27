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

    public Vector2 movementInput;
    public Vector2 changeElementInput;
    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool sprinting;
    public bool startJump;

    public bool attack1;
    bool attack1wasRealeased;
    public bool attack2;
    bool attack2wasRealeased;
    public bool attack3;
    bool attack3wasRealeased;

    Vector2[] pressedElement = { Vector2.up, Vector2.right, Vector2.down, Vector3.left };
    public int currentElementpressed;
    public bool elementalMovement;

    public enum Elements {Air, Fire, Electro, Water, Rock, Ice};
    public Elements[] playersElements = { Elements.Air, Elements.Fire, Elements.Electro, Elements.Water, Elements.Rock, Elements.Ice };
    public Elements currentElement;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        combatManager = GetComponent<CombatManager>();
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
            playerControls.CharacterControls.ChangeElement.performed += OnChangeElement;

            
            playerControls.Combat.Attack1.performed += OnAttack1;
            playerControls.Combat.Attack2.performed += OnAttack2;
            playerControls.Combat.Attack3.performed += OnAttack3;
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

    void OnAttack1(InputAction.CallbackContext context)
    {
        attack1 = context.ReadValueAsButton();
    }

    void OnAttack2(InputAction.CallbackContext context)
    {
        attack2 = context.ReadValueAsButton();
    }

    void OnAttack3(InputAction.CallbackContext context)
    {
        attack3 = context.ReadValueAsButton();
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
        HandleCombatInput();
        HandleJumpInput();
    }

    private void HandleCombatInput()
    {
        if (attack1)
        {
            combatManager.Attack2();
            attack1 = false;
        }
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.isSprinting);
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
}
