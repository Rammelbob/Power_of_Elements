using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    InputManager inputManager;
    AnimatorManager animatorManager;
    Rigidbody rb;
    CombatManager combatManager;

    FreeClimb freeClimb;
    FreeClimbAnimHook a_hook;

    Vector3 moveDirection;
    public Transform direction;
    public Transform body;
    public Transform groundCheck;
    public GameObject waterMovement;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float elementalAirFallingVelocity;
    public float rayCastHeight;
    Vector3 fallingTargetPosition;
    float groundDistance = 0.3f;
    public LayerMask groundLayer;

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;
    public bool isClimbing;

    [Header("Movement Speeds")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5;
    public float sprintingSpeed = 7;
    public float rotationSpeed = 15;
    public float staminaUsedSprinting;

    [Header("Jump Speeds")]
    public float jumpHeight = 3;
    public float gravityIntensity = -15;
    public float staminaUsedJumping;

    [Header("Climbing")]
    public float firstWallCheckDistance;
    public float climbCheckDistance;
    public float climbingRotationSpeed;
    public float wallDistance;
    public float wallCheckLeftandRigth;
    bool isWallRight, isWallLeft;
    RaycastHit wallFrontHit;
    RaycastHit moveDirHit;
    RaycastHit hitaroundCorner;
    Vector3 climbingRotation;
    Vector3 climbDirection;
    public LayerMask wallLayer;

    [Header("ElemetalMovement")]
    Elements lastElement;
    bool doAirMovement;
    bool doFireMovement;
    bool doElectroMovement;
    bool doEarthMovement;
    bool doWaterMovement;
    [Header("Electro")]
    public float electroDashSpeed;
    public float startDashTime;
    public float electroDashCooldown;
    float dashEndCooldowntime = 0;
    float dashTime;

    [Header("Water")]
    GameObject waterMovementtemp;

    [Header("Air")]
    public float staminaUsedAirMovement;
    public float elementalMovementSpeedAir;

    [Header("Fire")]
    public float staminaUsedFireMovement;
    public float elementalMovementSpeedFire;
    //[Header("Earth")]
    //[Header("Ice")]



    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        combatManager = GetComponent<CombatManager>();
        freeClimb = GetComponent<FreeClimb>();
        dashTime = startDashTime;
    }

    public void HandleAllMovement()
    {
        HandleElementalMovement();
        HandleFallingAndLanding();
        HandleRotation();
        if (playerManager.isInteracting)
            return;

        HandleWallClimbing();
        HandleMovement();
    }

    private bool IsDoingElementalMovement()
    {
        return doAirMovement || doEarthMovement || doElectroMovement || doFireMovement || doWaterMovement;
    }

    private void HandleElementalMovement()
    {
        if (lastElement != inputManager.currentElement)
        {
            doFireMovement = false;
            doAirMovement = false;
            doElectroMovement = false;
            doWaterMovement = false;
            doEarthMovement = false;
            combatManager.ResetCombo();
        }

        // ersetzen mit bool = element == Element && inputManager.ElementalMovement;
        if (inputManager.elementalMovement)
        {
            switch (inputManager.currentElement)
            {
                case Elements.Air:
                    doAirMovement = combatManager.CanUseStamina();
                    break;

                case Elements.Fire:
                    doFireMovement = combatManager.CanUseStamina();
                    break;

                case Elements.Electro:
                    if (!doElectroMovement && Time.time > dashEndCooldowntime)
                    {
                        dashEndCooldowntime = Time.time + electroDashCooldown;
                        playerManager.isInteracting = true;
                        doElectroMovement = true;
                    }
                    break;

                case Elements.Rock:
                    doEarthMovement = true;
                    break;

                case Elements.Water:
                    doWaterMovement = true;
                    break;

                case Elements.Ice:
                    break;

            }
        }
        else
        {
            doFireMovement = false;
            doAirMovement = false;
            //doElectroMovement = false;
            doWaterMovement = false;
            doEarthMovement = false;
        }

        if (doElectroMovement)
        {
            moveDirection = direction.forward * inputManager.verticalInput;
            moveDirection += direction.right * inputManager.horizontalInput;
            moveDirection.Normalize();
            if (moveDirection == Vector3.zero)
                moveDirection = body.forward;


            if (dashTime <= 0)
            {
                rb.velocity = Vector3.zero;
                dashTime = startDashTime;
                doElectroMovement = false;
                playerManager.isInteracting = false;
            }
            else
            {
                dashTime -= Time.deltaTime;
                rb.velocity = moveDirection * (isGrounded? electroDashSpeed : electroDashSpeed/5);
            }
        }
        else if(doWaterMovement)
        {
            if (waterMovementtemp == null)
            {
                waterMovementtemp = Instantiate(waterMovement, groundCheck.position, Quaternion.identity);
            }
        }
        else if (doEarthMovement)
        {

        }
        

        lastElement = inputManager.currentElement;
    }

    private void HandleMovement()
    {
        if (isJumping || isClimbing)
            return;

        moveDirection = direction.forward * inputManager.verticalInput;
        moveDirection += direction.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (isSprinting)
        {
            moveDirection *= sprintingSpeed;
            combatManager.UpdateStamina(-staminaUsedSprinting * Time.deltaTime);
        }
        else
        {
            if (inputManager.moveAmount >= 0.5f)
            {
                moveDirection *= runningSpeed;
            }
            else
            {
                moveDirection *= walkingSpeed;
            }
        }
        Vector3 movementVeolcity = moveDirection;
        rb.velocity = movementVeolcity;
    }

    private void HandleRotation()
    {
        if (isJumping)
            return;

        Quaternion targetRotation;
        Quaternion playerRotation = new Quaternion();
        Vector3 targetDirection = Vector3.zero;

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

        targetDirection = direction.forward * inputManager.verticalInput;
        targetDirection += direction.right * inputManager.horizontalInput;

        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = body.forward;

        targetRotation = Quaternion.LookRotation(targetDirection);
        if (isGrounded && !playerManager.isInteracting)
        {
            playerRotation = Quaternion.Slerp(body.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            playerRotation = Quaternion.Slerp(body.rotation, targetRotation, rotationSpeed/5 * Time.deltaTime);
        }
        

        body.rotation = playerRotation;
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        bool isOnMoveable = false;

        Vector3 rayCastOrigin = transform.position;//groundCheck.position;
        fallingTargetPosition = groundCheck.position;
        //rayCastOrigin.y += rayCastHeightOffset;

        if (!isClimbing)
        {
            if (!isGrounded && !isJumping)
            {
                if (!playerManager.isInteracting)
                {
                    animatorManager.PlayTargetAnimation("Falling", true, 0.2f, false);
                }

                if (doFireMovement)
                {
                    inAirTimer = 0;
                    moveDirection = direction.forward * inputManager.verticalInput;
                    moveDirection += direction.right * inputManager.horizontalInput;
                    moveDirection.Normalize();
                    rb.AddForce(moveDirection * Time.deltaTime * elementalMovementSpeedFire, ForceMode.Acceleration);
                    combatManager.UpdateStamina(-staminaUsedFireMovement * Time.deltaTime);
                }
                else if (doAirMovement)
                {
                    inAirTimer = 0;
                    moveDirection = direction.forward * inputManager.verticalInput;
                    moveDirection += direction.right * inputManager.horizontalInput;
                    moveDirection.Normalize();
                    rb.AddForce(-Vector3.up * elementalAirFallingVelocity * Time.deltaTime);
                    rb.AddForce(moveDirection * Time.deltaTime * elementalMovementSpeedAir, ForceMode.Acceleration);
                    combatManager.UpdateStamina(-staminaUsedAirMovement * Time.deltaTime);
                }
                else
                {
                    inAirTimer += Time.deltaTime;
                    rb.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
                }
            }
        }

        //if (Physics.CheckSphere(animatorManager.animator.GetBoneTransform(HumanBodyBones.LeftFoot).position, groundDistance, groundLayer)
          //  || Physics.CheckSphere(animatorManager.animator.GetBoneTransform(HumanBodyBones.RightFoot).position, groundDistance, groundLayer))
        if (Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer))
        {
            if (!isGrounded && !playerManager.isInteracting)
                animatorManager.PlayTargetAnimation("Landing", true, 0.2f, false);

            if (Physics.Raycast(rayCastOrigin, -Vector3.up, out hit, rayCastHeight, groundLayer))
            {
                Vector3 rayCastHitPoint = hit.point;
                fallingTargetPosition.y = rayCastHitPoint.y + (transform.position.y - groundCheck.position.y);
            }
            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            //if (Physics.Raycast(rayCastOrigin, -Vector3.up, out hit, rayCastHeight, groundLayer))
            //{
            //    Vector3 rayCastHitPoint = hit.point;
            //    fallingTargetPosition.y = rayCastHitPoint.y + (transform.position.y - groundCheck.position.y);
            //    inAirTimer = 0;
            //    isGrounded = true;
            //}
            //else
                isGrounded = false;
        }
        
        if (isGrounded && !isJumping && !isClimbing && !isOnMoveable && !IsDoingElementalMovement())
        {
            transform.position = Vector3.Lerp(transform.position, fallingTargetPosition, Time.deltaTime / 0.1f);
            //if (playerManager.isInteracting || inputManager.moveAmount > 0)
            //{
            //    transform.position = Vector3.Lerp(transform.position, fallingTargetPosition, Time.deltaTime / 0.1f);
            //}
            //else if (transform.position.y < fallingTargetPosition.y)
            //{
            //    transform.position = fallingTargetPosition;
            //}

        }
    }


    public void HandleJumping()
    {
        if (isGrounded && !isClimbing && !playerManager.isInteracting && combatManager.CanUseStamina())
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false, 0.1f, false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            rb.velocity = playerVelocity;
            combatManager.UpdateStamina(-staminaUsedJumping);
        }
    }

    public void HandleDodgeing()
    {
        if (isGrounded && !isClimbing && !playerManager.isInteracting && combatManager.CanUseStamina())
        {
            rb.velocity = Vector3.zero;
            animatorManager.PlayTargetAnimation("Dodge", true, 0.1f, true);
            combatManager.UpdateStamina(-staminaUsedJumping);
        }
    }

    void HandleWallClimbing()
    {
        if (!isClimbing)
        {
            if (Physics.Raycast(transform.position, body.forward, out wallFrontHit, firstWallCheckDistance, wallLayer) && !isGrounded)
            {
                climbingRotation = -wallFrontHit.normal;

                Debug.DrawLine(transform.position, transform.position + body.forward * firstWallCheckDistance, Color.red, 0.2f);

                animatorManager.PlayTargetAnimation("Climbing", false, 0.1f, true);
                isClimbing = true;
                rb.velocity = Vector3.zero;
                SetPositionFromWall(wallFrontHit.point, wallFrontHit.normal);
            }
        }
        else
        {
            climbDirection = body.up * inputManager.verticalInput;
            climbDirection += body.right * inputManager.horizontalInput;
            climbDirection.Normalize();

            ClimbDirection(climbDirection);
            if (isGrounded)
                isClimbing = false;
        }
       
    }

    private void ClimbDirection(Vector3 moveDir)
    {
        Vector3 origin = transform.position;
        Vector3 rayDir = moveDir;

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
                SetPositionFromWall(moveDirHit.point, moveDirHit.normal);
                climbingRotation = -moveDirHit.normal;
            }
            return;
        }


        origin += rayDir * climbCheckDistance;
        rayDir = -moveDir;

        Debug.DrawLine(origin, origin + rayDir * (firstWallCheckDistance + 0.1f), Color.black, 0.2f);
        if (Physics.Raycast(origin, rayDir, out moveDirHit, (firstWallCheckDistance + 0.1f), wallLayer))
        {
            if (inputManager.verticalInput > 0 && Vector3.Angle(Vector3.up, moveDirHit.normal) < 45)
            {
                animatorManager.PlayTargetAnimation("ClimbingOverEdge", true, 0.1f, true);
            }
            else if (CheckForCorner())
            {
                FindCorner(origin, rayDir);
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

    private void FindCorner(Vector3 origin ,Vector3 rayDir)
    {
        origin += -body.forward * (wallDistance - 0.35f);
        RaycastHit cornerHit;
        if (Physics.Raycast(origin, rayDir, out cornerHit, firstWallCheckDistance, wallLayer))
        {
            SetPositionFromWall(cornerHit.point, cornerHit.normal);
            return;
        }

        SetPositionFromWall(moveDirHit.point, moveDirHit.normal);
    }

    private bool CheckForCorner()
    {
        Debug.DrawLine(transform.position + body.right * wallCheckLeftandRigth, (transform.position + body.right * wallCheckLeftandRigth) + body.forward * climbCheckDistance, Color.green, 0.2f);
        isWallRight = Physics.Raycast(transform.position + body.right * wallCheckLeftandRigth, body.forward, climbCheckDistance, wallLayer);

        Debug.DrawLine(transform.position + -body.right * wallCheckLeftandRigth, (transform.position + -body.right * wallCheckLeftandRigth) + body.forward * climbCheckDistance, Color.green, 0.2f);
        isWallLeft = Physics.Raycast(transform.position + -body.right * wallCheckLeftandRigth, body.forward, climbCheckDistance, wallLayer);

        return isWallLeft != isWallRight;
    }

    //private void OnDrawGizmos()
    //{
    //    Vector3 a = Vector3.forward, b = (Vector3.right+Vector3.forward).normalized, c = Vector3.Cross(a,b);

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(Vector3.zero, Vector3.zero + a);

    //    Gizmos.color = Color.green;
    //    Gizmos.DrawLine(Vector3.forward, Vector3.forward + b);

    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawLine(Vector3.zero, Vector3.zero + c);
    //}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, 0.3f);
    }
}
