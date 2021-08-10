using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementScript : MonoBehaviour
{
    #region Variables
    PlayerInput playerInput;
    [Header("Speeds")]
    public float walkingSpeed;
    public float runningSpeed;
    public float acceleration;
    public float airMultipier;
    public float moveMultiplier;


    [Header("Physics")]
    public float jumpForce;


    [Header("External")]
    public LayerMask discludePlayer, groundMask;
    public Transform direction;
    public Transform body;
    public Transform groundCheck;
    public Animator anim;
    Rigidbody rb;
    

    [Header("Surface Control")]
    public float slopeClimbingSpeed;
    public float slopeDecendSpeed;


    private bool isGrounded, isClimbing, isMovementPressed, isRunning, startJump;
    private float delta;
    private float wallDistance = 0.3f;
    private float rotationSpeed = 30;
    private float movementSpeed;
    private Vector3 moveVector;
    private Vector2 currentMovemntInput;
    private Vector3 currentMoveDirection;
    enum Animtionstate { Idle, Walking, Running, IdleJump, MovementJump, Climbing }
    Animtionstate currentAnimtionstate;

    #endregion

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Run.started += OnRun;
        playerInput.CharacterControls.Run.canceled += OnRun;
        playerInput.CharacterControls.Jump.started += OnJump;
        currentAnimtionstate = Animtionstate.Idle;
        rb = GetComponent<Rigidbody>();
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

    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }

    //private void Update()
    //{
    //    currentMoveDirection.x = Input.GetAxisRaw("Horizontal");
    //    currentMoveDirection.z = Input.GetAxisRaw("Vertical");
    //    isMovementPressed = currentMoveDirection.x != 0 || currentMoveDirection.z != 0;

    //    isRunning = Input.GetKey(KeyCode.LeftShift);

    //    startJump = Input.GetButton("Jump");
    //}

    private void FixedUpdate()
    {
        Gravity();
        Jump();
        MovmentSpeed();
        Move();
        VelocitClamp();
        Rotation();
        Animations();
    }

    private void Gravity()
    {
        delta = Time.deltaTime;
        isGrounded = Physics.CheckSphere(groundCheck.position, wallDistance, groundMask);

        if (rb.useGravity)
            rb.AddForce(Physics.gravity * rb.mass * 1.5f);

    }

    private void Jump()
    {
        if (startJump && isGrounded)
        {
            rb.AddForce(body.up * jumpForce, ForceMode.Impulse);
            startJump = false;
        }
    }

    void MovmentSpeed()
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

    private void Move()
    {
        moveVector = currentMoveDirection.z * direction.forward + currentMoveDirection.x * direction.right;
        moveVector.Normalize();
        if (!isGrounded)
        {
            moveVector *= airMultipier;
        }
        Vector3 movement = moveVector * delta * moveMultiplier * movementSpeed;
        rb.AddForce(movement, ForceMode.Acceleration);
    }

    private void VelocitClamp()
    {
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -runningSpeed, runningSpeed), Mathf.Clamp(rb.velocity.y, -50f, jumpForce), Mathf.Clamp(rb.velocity.z, -runningSpeed, runningSpeed));
    }

    void Rotation()
    {
        //if (isClimbing)
        //{
        //    return;
        //}
        Vector3 positionTolookAt = new Vector3(moveVector.x, 0, moveVector.z);

        Quaternion currentrotation = body.rotation;

        if (isMovementPressed && positionTolookAt != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionTolookAt);
            body.rotation = Quaternion.Slerp(currentrotation, targetRotation, rotationSpeed * delta);
        }
    }
    void Animations()
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
        else if (!isClimbing)
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
                    anim.CrossFade("Idle", 0.01f);
                    break;
                case Animtionstate.Walking:
                    anim.CrossFade("Walking", 0.01f);
                    break;
                case Animtionstate.Running:
                    anim.CrossFade("Running", 0.01f);
                    break;
                case Animtionstate.IdleJump:
                    anim.CrossFade("IdleJump", 0.1f);
                    break;
                case Animtionstate.MovementJump:
                    anim.CrossFade("MovementJump", 0.1f);
                    break;
            }
        }
    }
}
