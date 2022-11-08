using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    public Rigidbody rb;

    Vector3 moveDirection;
    public Transform direction;
    public Transform body;
    public Transform groundCheck;

    [Header("Falling")]
    public float gravityMultiplier;
    public float groundCheckDistance;
    public float distanceToGround;
    public LayerMask groundLayer;
    RaycastHit hit;
    Vector3 rayCastOrigin;
    Vector3 fallingTargetPosition;
    Vector3 rayCastHitPoint;

    [Header("Rotate")]
    Quaternion targetRotation;
    Quaternion playerRotation;
    Vector3 targetDirection;

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isClimbing;

    [Header("Movement Speeds")]
    public float movementMultiplier;
    public float rotationSpeed;
    public float staminaUsedSprinting;

    [Header("Jump")]
    public float staminaUsedJumping;
    public float jumpUpForce;
    public float jumpForwardForce;

    [Header("Climbing")]
    public float firstWallCheckDistance;
    public float climbCheckDistance;
    public float climbingRotationSpeed;
    public float wallDistance;
    public float wallCheckLeftandRigth;
    bool isWallRight, isWallLeft;
    RaycastHit wallFrontHit;
    RaycastHit moveDirHit;
    Vector3 climbingRotation;
    Vector3 climbDirection;
    Vector3 origin;
    Vector3 rayDir;
    Vector3 cornerOrigin;
    public LayerMask wallLayer;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();
        HandleRotation();
       
        if (playerManager.isInteracting)
            return;

        HandleWallClimbing();
    }

    private void HandleRotation()
    {
        if (!playerManager.canRotate)
            return;


        playerRotation = new Quaternion();
        targetDirection = Vector3.zero;

        if (isClimbing)
        {
            targetDirection = climbingRotation;
            if (targetDirection == Vector3.zero)
                targetDirection = body.forward;

            targetRotation = Quaternion.LookRotation(targetDirection);
            playerRotation = Quaternion.Slerp(body.rotation, targetRotation, climbingRotationSpeed * Time.deltaTime);
            body.rotation = playerRotation;

            return;
        }

        SetMoveDir();
        targetDirection = moveDirection;
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = body.forward;

        targetRotation = Quaternion.LookRotation(targetDirection);
        playerRotation = Quaternion.Slerp(body.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        body.rotation = playerRotation;
    }

    private void HandleFallingAndLanding()
    {
        rayCastOrigin = transform.position;
        fallingTargetPosition = groundCheck.position;


        if (Physics.Raycast(rayCastOrigin, -Vector3.up, out hit, groundCheckDistance, groundLayer))
        {
            distanceToGround = hit.distance - (transform.position.y - groundCheck.position.y);

            if (distanceToGround < 0.35f)
            {
                rayCastHitPoint = hit.point;
                fallingTargetPosition.y = rayCastHitPoint.y + (transform.position.y - groundCheck.position.y);
                if (!isGrounded)
                {
                    playerManager.playerAnimatorManager.PlayPlayerTargetAnimation("Locomotion", false, 0.05f, true, false);
                }
                isGrounded = true;
                rb.useGravity = false;
            }
            else
            {
                isGrounded = false;
                rb.useGravity = true;
            }
                
        }
        else
        {
            isGrounded = false;
            rb.useGravity = true;
            distanceToGround = groundCheckDistance;
        }

        if (!isClimbing)
        {
            if (!isGrounded)
            {
                if (!playerManager.isInteracting && rb.velocity.y < -3f)
                {
                    SetMoveDir();
                    rb.AddForce(moveDirection * jumpForwardForce, ForceMode.Impulse);
                    playerManager.playerAnimatorManager.PlayPlayerTargetAnimation("Falling", true, 0.1f, false, false);
                }

                if (rb.useGravity)
                    rb.AddForce(Physics.gravity * rb.mass * gravityMultiplier);
            }
            else
            {
                if (playerManager.inputManager.moveAmount > 0.5f)
                    transform.position = Vector3.Lerp(transform.position, fallingTargetPosition, Time.deltaTime * 25);
                else
                    transform.position = fallingTargetPosition;
            }
        }
        else
        {
            rb.useGravity = false;
        }
    }

    public void SetMoveDir()
    {
        moveDirection = direction.forward * playerManager.inputManager.verticalInput;
        moveDirection += direction.right * playerManager.inputManager.horizontalInput;
        moveDirection.Normalize();
    }

    public void HandleJumping()
    {
        if (isGrounded && !isClimbing && !playerManager.isInteracting)
        {
            if (playerManager.playerStats.CanUseStamina(staminaUsedJumping))
            {
                if (playerManager.inputManager.moveAmount > 0.5)
                {
                    playerManager.playerAnimatorManager.PlayPlayerTargetAnimation("Jump", false, 0.2f, false, false);
                    playerManager.playerStats.GetStatByEnum(StatEnum.Stamina).statvalues.ChangeCurrentStat(-staminaUsedJumping);
                    SetMoveDir();
                    rb.AddForce(moveDirection * jumpForwardForce, ForceMode.Impulse);
                    rb.AddForce(body.up * jumpUpForce, ForceMode.Impulse);
                }
                else
                    playerManager.playerAnimatorManager.PlayPlayerTargetAnimation("Jumping_Idle", false, 0.1f, true, true);
            }
        }
    }

    public void HandleDodgeing()
    {
        if (isGrounded && !isClimbing && !playerManager.isInteracting)
        {
            if (playerManager.playerStats.CanUseStamina(staminaUsedJumping))
            {
                if (playerManager.inputManager.moveAmount > 0.5)
                {
                    playerManager.playerAnimatorManager.PlayPlayerTargetAnimation("Dodge", true, 0.1f, true, true);
                    playerManager.playerStats.GetStatByEnum(StatEnum.Stamina).statvalues.ChangeCurrentStat(-staminaUsedJumping);
                }
                else
                {
                    playerManager.playerAnimatorManager.PlayPlayerTargetAnimation("Dodge_Back", true, 0.1f, true, true);
                    playerManager.playerStats.GetStatByEnum(StatEnum.Stamina).statvalues.ChangeCurrentStat(-staminaUsedJumping);
                }
            }
        }
    }

    void HandleWallClimbing()
    {
        if (!isClimbing)
        {
            if (Physics.Raycast(transform.position, body.forward, out wallFrontHit, firstWallCheckDistance, wallLayer) && !isGrounded)
            {
                climbingRotation = -wallFrontHit.normal;

                playerManager.playerAnimatorManager.PlayPlayerTargetAnimation("Climbing", false, 0.1f, true, true);
                isClimbing = true;
                SetPositionFromWall(wallFrontHit.point, wallFrontHit.normal);
            }
        }
        else
        {
            climbDirection = body.up * playerManager.inputManager.verticalInput;
            climbDirection += body.right * playerManager.inputManager.horizontalInput;
            climbDirection.Normalize();

            ClimbDirection(climbDirection);
            if (isGrounded)
                isClimbing = false;
        }
       
    }

    private void ClimbDirection(Vector3 moveDir)
    {
        origin = transform.position;
        rayDir = moveDir;
        

        Debug.DrawLine(origin, origin + rayDir * firstWallCheckDistance, Color.blue, 0.2f);
        if (Physics.Raycast(origin, rayDir, out moveDirHit, firstWallCheckDistance, wallLayer))
        {
            SetPositionFromWall(moveDirHit.point, moveDirHit.normal);
            climbingRotation = -moveDirHit.normal;
            return;
        }

        origin += rayDir * firstWallCheckDistance;
        rayDir = body.forward;

        Debug.DrawLine(origin, origin + rayDir * climbCheckDistance, Color.yellow, 0.2f);
        if (Physics.Raycast(origin, rayDir, out moveDirHit, climbCheckDistance, wallLayer))
        {
            if (Physics.Raycast(transform.position, body.forward, out moveDirHit, firstWallCheckDistance, wallLayer))
            {
                //SetPositionFromWall(moveDirHit.point, moveDirHit.normal,Color.blue);
                climbingRotation = -moveDirHit.normal;
            }
            return;
        }

        cornerOrigin = origin;
        cornerOrigin += rayDir * (firstWallCheckDistance);
        origin += rayDir * climbCheckDistance;
        rayDir = -moveDir;

        Debug.DrawLine(origin, origin + rayDir * (firstWallCheckDistance + 0.1f), Color.black, 0.2f);
        if (Physics.Raycast(origin, rayDir, out moveDirHit, (firstWallCheckDistance + 0.1f), wallLayer))
        {
            if (playerManager.inputManager.verticalInput > 0 && Vector3.Angle(Vector3.up, moveDirHit.normal) < 45)
            {
                playerManager.playerAnimatorManager.PlayPlayerTargetAnimation("ClimbingOverEdge", true, 0.1f, true, true);
            }
            else if (CheckForCorner())
            {
                FindCorner(cornerOrigin, rayDir);
                climbingRotation = -moveDirHit.normal;
            }
            return;
        }

        isClimbing = false;
    }

    private void SetPositionFromWall(Vector3 hitPoint,Vector3 hitNormal)
    {
        transform.position = hitPoint + hitNormal * wallDistance;
        rb.velocity = Vector3.zero;
    }

    private void FindCorner(Vector3 cornerOrigin ,Vector3 rayDir)
    {
        RaycastHit cornerHit;
        if (Physics.Raycast(cornerOrigin, rayDir, out cornerHit, firstWallCheckDistance, wallLayer))
        {
            SetPositionFromWall(cornerHit.point, cornerHit.normal);
        }  
    }

    private bool CheckForCorner()
    {
        Debug.DrawLine(transform.position + body.right * wallCheckLeftandRigth, (transform.position + body.right * wallCheckLeftandRigth) + body.forward * climbCheckDistance, Color.green, 0.2f);
        isWallRight = Physics.Raycast(transform.position + body.right * wallCheckLeftandRigth, body.forward, climbCheckDistance, wallLayer);

        Debug.DrawLine(transform.position + -body.right * wallCheckLeftandRigth, (transform.position + -body.right * wallCheckLeftandRigth) + body.forward * climbCheckDistance, Color.green, 0.2f);
        isWallLeft = Physics.Raycast(transform.position + -body.right * wallCheckLeftandRigth, body.forward, climbCheckDistance, wallLayer);

        return isWallLeft != isWallRight;
    }
}
