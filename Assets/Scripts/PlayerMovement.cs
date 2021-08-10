using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    CharacterController characterController;
    public Animator animator;
    public Transform body;
    public Transform direction;
    public Transform wallCheck;
    public FreeClimb freeClimb;
    public FreeClimbAnimHook a_hook;

    Vector2 currentMovemntInput;
    Vector3 currentMoveDirection;
    Vector3 currentGravity;
    Vector3 moveDirectionWithCameraDirection;
    bool isMovementPressed;
    bool isRunning;
    bool startJump;
    bool isGrounded;
    public bool isClimbing;
    public Transform groundCheck;
    float groundDistance = 0.1f;
    public LayerMask groundMask;
    public LayerMask wallMask;

    float movementSpeed;
    float walkingSpeed = 4f;
    float runningSpeed = 7f;
    float acceleration = 5;
    float rotaionSpeed = 15;
    float jumpHeight = 5;
    float gravity;
    float delta;


    enum Animtionstate {Idle, Walking, Running, IdleJump, MovementJump, Climbing}
    Animtionstate currentAnimtionstate;


    private void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Run.started += OnRun;
        playerInput.CharacterControls.Run.canceled += OnRun;
        playerInput.CharacterControls.Jump.started += OnJump;
        currentAnimtionstate = Animtionstate.Idle;
    }

    void OnRun(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
    }

    void OnJump(InputAction.CallbackContext context)
    {
        startJump = context.ReadValueAsButton();   
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovemntInput = context.ReadValue<Vector2>();
        currentMoveDirection.x = currentMovemntInput.x;
        currentMoveDirection.z = currentMovemntInput.y;
        isMovementPressed = currentMovemntInput.x != 0 || currentMovemntInput.y != 0;
    }

    void HandleAnimations()
    {
        Animtionstate animtionstateTemp = currentAnimtionstate;
        if (isGrounded && !isClimbing)
        {
            if (isMovementPressed && !isRunning && currentAnimtionstate != Animtionstate.Walking)
            {
                currentAnimtionstate = Animtionstate.Walking;
            }
            else if (isMovementPressed && isRunning && currentAnimtionstate != Animtionstate.Running)
            {
                currentAnimtionstate = Animtionstate.Running;
            }
            else if (!isMovementPressed && currentAnimtionstate != Animtionstate.Idle)
            {
                currentAnimtionstate = Animtionstate.Idle;
            }
        }
        else if(!isClimbing)
        {
            if (isMovementPressed)
            {
                currentAnimtionstate = Animtionstate.MovementJump;
            }
            else if (!isMovementPressed && currentAnimtionstate == Animtionstate.Idle)
            {
                currentAnimtionstate = Animtionstate.IdleJump;
            }
        }
       

        if (animtionstateTemp != currentAnimtionstate)
        {
            switch (currentAnimtionstate)
            {
                case Animtionstate.Idle:
                    animator.CrossFade("Idle", 0.01f);
                    break;
                case Animtionstate.Walking:
                    animator.CrossFade("Walking", 0.01f);
                    break;
                case Animtionstate.Running:
                    animator.CrossFade("Running", 0.01f);
                    break;
                case Animtionstate.IdleJump:
                    animator.CrossFade("IdleJump", 0.1f);
                    break;
                case Animtionstate.MovementJump:
                    animator.CrossFade("MovementJump",0.1f);
                    break;
            }
        }
    }

    void HandleRotation()
    {
        if (isClimbing)
        { 
            return;
        }
        Vector3 positionTolookAt = new Vector3(moveDirectionWithCameraDirection.x, 0, moveDirectionWithCameraDirection.z);

        Quaternion currentrotation = body.rotation;

        if (isMovementPressed && positionTolookAt != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionTolookAt);
            body.rotation = Quaternion.Slerp(currentrotation, targetRotation, rotaionSpeed * delta);
        }
    }

    void HandleGravity()
    {
        if (characterController.isGrounded || isClimbing)
        {
            gravity = -2f;
            currentGravity.y = gravity;
        }
        else
        {
            gravity = -9.81f;
            currentGravity.y += gravity * delta;
        }

    }

    void HandleJump()
    {
        if (startJump && isGrounded)
        {
            currentGravity.y = jumpHeight;
        }
        startJump = false;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        delta = Time.deltaTime;
        HandleMovmentSpeed();
        HandleGravity();
        HandleSlope();
        HandleJump();
        HandleWallClimbing();
        HandelMovement();
        HandleRotation();
        HandleAnimations();
    }

    void HandleMovmentSpeed()
    {
        if (isRunning)
        {
            movementSpeed = Mathf.Lerp(movementSpeed, runningSpeed, acceleration * delta);
        }
        else
        {
            movementSpeed = Mathf.Lerp(movementSpeed, walkingSpeed, acceleration * delta);
        }
    }

    void HandelMovement()
    {
        if (!isClimbing)
        {
            moveDirectionWithCameraDirection = currentMoveDirection.z * direction.forward + currentMoveDirection.x * direction.right;
            characterController.Move((moveDirectionWithCameraDirection.normalized * movementSpeed + currentGravity) * delta);
        }
    }

    void HandleWallClimbing()
    {
        if (isClimbing)
        {
            a_hook.enabled = true;
            freeClimb.Tick(delta, currentMovemntInput);
        }
       
        if (isGrounded)
        {
            if (isClimbing)
            {
                isClimbing = false;
                a_hook.enabled = false;
            }
        }
        else if (!isGrounded && !isClimbing)
        {
            isClimbing = freeClimb.CheckForClimb();
            if (isClimbing)
            {
                currentAnimtionstate = Animtionstate.Climbing;
                freeClimb.Tick(delta, currentMovemntInput);
            }
        }
    }
    //bool  WallDetection()
    //{
    //    return Physics.BoxCast(wallCheck.position, new Vector3(0.07f, 0.5f, 0.07f), body.forward, body.rotation, characterController.radius + groundDistance, wallMask);
    //}

    void HandleSlope()
    {
        if (!characterController.isGrounded)
            return;

        if (Physics.Raycast(transform.position, Vector3.down, (Vector3.Distance(groundCheck.position, transform.position) + groundDistance), groundMask))
        {
            currentGravity.y = -9.81f;
        }
    }

    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }

    //private void OnDrawGizmos()
    //{
    //    //Gizmos.DrawLine(wallCheck.position, wallCheck.position + body.forward * (0.2f + groundDistance));
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(wallCheck.position + body.forward * groundDistance * 2.1f, new Vector3(0.2f, 1, 0.2f));
    //}
}
