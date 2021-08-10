using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    InputManager inputManager;
    AnimatorManager animatorManager;
    Rigidbody rb;

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
    public float rayCastHeightOffset;
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
    public float elementalMovementSpeedAir;
    public float elementalMovementSpeedFire;

    [Header("Jump Speeds")]
    public float jumpHeight = 3;
    public float gravityIntensity = -15;

    [Header("Climbing")]
    public Transform wallCheck;
    public float wallDistance;
    float climbingSpeed = 1f;
    public float climbCheckDistance;
    public float climbingRotationSpeed;
    bool onCorner;
    bool turnCorner;
    RaycastHit wallFrontHit;
    RaycastHit moveDirHit;
    Vector3 climbingRotation;
    Vector3 cornerTurningRotation;
    public LayerMask wallLayer;
    float t;
    float offSetFormWall = 0.5f;
    Vector3 startPosition;
    Vector3 targetPosition;
    Quaternion startRotation;

    [Header("ElemetalMovement")]
    InputManager.Elements lastElement;
    bool doAirMovement;
    bool doFireMovement;
    bool doElectroMovement;
    public float electroDashSpeed;
    float dashTime;
    public float startDashTime;
    bool doWaterMovement;
    GameObject waterMovementtemp;
    bool doRockMovement;


    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        freeClimb = GetComponent<FreeClimb>();
        dashTime = startDashTime;
    }

    public void HandleAllMovement()
    {
        HandleElementalMovement();
        HandleFallingAndLanding();
        if (playerManager.isInteracting)
            return;

        HandleWallClimbing();
        HandleMovement();
        HandleRotation();
    }

    private void HandleElementalMovement()
    {
        if (lastElement != inputManager.currentElement)
        {
            doFireMovement = false;
            doAirMovement = false;
            doElectroMovement = false;
            doWaterMovement = false;
            doRockMovement = false;
        }

        // ersetzen mit bool = element == Element && inputManager.ElementalMovement;
        if (inputManager.elementalMovement)
        {
            switch (inputManager.currentElement)
            {
                case InputManager.Elements.Air:
                    doAirMovement = true;
                    break;

                case InputManager.Elements.Fire:
                    doFireMovement = true;
                    break;

                case InputManager.Elements.Electro:
                    playerManager.isInteracting = true;
                    doElectroMovement = true;
                    break;

                case InputManager.Elements.Rock:
                    doRockMovement = true;
                    break;

                case InputManager.Elements.Water:
                    playerManager.isInteracting = true;
                    doWaterMovement = true;
                    break;

                case InputManager.Elements.Ice:
                    break;

            }
        }
        else
        {
            doFireMovement = false;
            doAirMovement = false;
            doElectroMovement = false;
            doWaterMovement = false;
            doRockMovement = false;
        }

        if (doElectroMovement)
        {
            moveDirection = direction.forward * inputManager.verticalInput;
            moveDirection += direction.right * inputManager.horizontalInput;
            moveDirection.Normalize();
            if (moveDirection == Vector3.zero)
                moveDirection = direction.forward;


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
                rb.velocity = moveDirection * electroDashSpeed;
            }
        }
        else if(doWaterMovement)
        {
            if (waterMovementtemp == null)
            {
                waterMovementtemp = Instantiate(waterMovement, groundCheck.position, Quaternion.identity);
            }
        }
        else if (doRockMovement)
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

        if (isClimbing)// && inputManager.moveAmount > 0)
        {
            targetDirection = climbingRotation;
            if (targetDirection == Vector3.zero)
                targetDirection = body.forward;

            if (turnCorner)
            {
                targetRotation = Quaternion.LookRotation(targetDirection);
                playerRotation = Quaternion.Slerp(startRotation, targetRotation, t);
            }
            else
            {
                targetRotation = Quaternion.LookRotation(targetDirection);
                playerRotation = Quaternion.Slerp(body.rotation, targetRotation, climbingRotationSpeed * Time.deltaTime);

            }

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
        playerRotation = Quaternion.Slerp(body.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        body.rotation = playerRotation;
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;

        Vector3 rayCastOrigin = groundCheck.position;
        Vector3 targetPosition = groundCheck.position;
        rayCastOrigin.y += rayCastHeightOffset;

        if (!isClimbing && !turnCorner)
        {
            if (!isGrounded && !isJumping)
            {
                if (!playerManager.isInteracting)
                {
                    animatorManager.PlayTargetAnimation("Falling", true, 0.2f);
                }

                if (doFireMovement)
                {
                    moveDirection = direction.forward * inputManager.verticalInput;
                    moveDirection += direction.right * inputManager.horizontalInput;
                    moveDirection.Normalize();
                    rb.AddForce(moveDirection * Time.deltaTime * elementalMovementSpeedFire, ForceMode.Acceleration);

                }
                else if (doAirMovement)
                {
                    moveDirection = direction.forward * inputManager.verticalInput;
                    moveDirection += direction.right * inputManager.horizontalInput;
                    moveDirection.Normalize();
                    rb.AddForce(-Vector3.up * elementalAirFallingVelocity * Time.deltaTime);
                    rb.AddForce(moveDirection * Time.deltaTime * elementalMovementSpeedAir, ForceMode.Acceleration);
                }
                else if(!doWaterMovement)
                {
                    inAirTimer += Time.deltaTime;
                    rb.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
                }
            }
        }

        //if (Physics.CheckSphere(animatorManager.animator.GetBoneTransform(HumanBodyBones.LeftFoot).position, groundDistance, groundLayer)
          //  || Physics.CheckSphere(animatorManager.animator.GetBoneTransform(HumanBodyBones.RightFoot).position, groundDistance, groundLayer))
        if (Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer) && !turnCorner)
        {
            
            Physics.Raycast(rayCastOrigin, -Vector3.up, out hit, rayCastHeightOffset + groundDistance, groundLayer);
            if (!isGrounded && !playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Landing", true, 0.2f);
            }

            Vector3 rayCastHitPoint = hit.point;
            targetPosition.y = rayCastHitPoint.y +(transform.position.y - groundCheck.position.y);
             
            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded && !isJumping && !isClimbing)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            //if (playerManager.isInteracting || inputManager.moveAmount > 0)
            //{
            //    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            //}
            //else if (transform.position.y < targetPosition.y)
            //{
            //    transform.position = targetPosition;
            //}

        }
    }

    public void HandleJumping()
    {
        if (isGrounded && !isClimbing)
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false, 0.1f);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            rb.velocity = playerVelocity;
        }
    }

    void HandleWallClimbing()
    {
        if (Physics.Raycast(transform.position, body.forward, out wallFrontHit, wallDistance, wallLayer) && !isGrounded)
        {
            climbingRotation = -wallFrontHit.normal;
            Debug.DrawLine(transform.position, transform.position + body.forward * wallDistance, Color.red, 0.2f);
            if (!isClimbing)
            {
                animatorManager.PlayTargetAnimation("Ledge_Idle", false, 0.1f);
                isClimbing = true;
            }
        }
        else if (onCorner && !turnCorner)
        {
            t = 0;
            turnCorner = true;
            startRotation = body.rotation;
            startPosition = transform.position;
            targetPosition = moveDirHit.point + moveDirHit.normal * offSetFormWall;
        }
        else if(!turnCorner)
            isClimbing = false;

        onCorner = false;

        if (isClimbing && !turnCorner)
        {
            moveDirection = body.up * inputManager.verticalInput;
            moveDirection += body.right * inputManager.horizontalInput;
            moveDirection.Normalize();

            if (ClimbDirection(moveDirection))
            {
                moveDirection *= climbingSpeed;

                Vector3 climbVeolcity = moveDirection;
                rb.velocity = climbVeolcity;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
        else if(turnCorner)
        {
            t += Time.deltaTime * 5;
            if (t > 1)
            {
                t = 1;
                climbingRotation = cornerTurningRotation;
                turnCorner = false;
            }
            transform.position = Vector3.Slerp(startPosition, targetPosition, t);
        }
    }

    private bool ClimbDirection(Vector3 moveDir)
    {
        Vector3 origin = transform.position;
        Vector3 rayDir = moveDir;

        Debug.DrawLine(origin, origin + rayDir * climbCheckDistance, Color.blue, 0.2f);
        if (Physics.Raycast(origin, rayDir, out moveDirHit, climbCheckDistance, wallLayer))
        {
            climbingRotation = -moveDirHit.normal;
            return true;
        }

        origin += rayDir * climbCheckDistance;
        rayDir = body.forward;

        Debug.DrawLine(origin, origin + rayDir * wallDistance, Color.yellow, 0.2f);
        if (Physics.Raycast(origin, rayDir, out moveDirHit, wallDistance, wallLayer))
        {
            return true;
        }

        origin += rayDir * wallDistance;
        rayDir = -moveDir;

        Debug.DrawLine(origin, origin + rayDir * (climbCheckDistance + 0.1f), Color.black, 0.2f);
        if (Physics.Raycast(origin, rayDir, out moveDirHit, (climbCheckDistance + 0.1f), wallLayer))
        {
            cornerTurningRotation = -moveDirHit.normal;
            onCorner = true;
            return true;
        }

        return false;
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

    //void HandleWallClimbing()
    //{
    //    if (Physics.Raycast(transform.position, body.forward, out wallFrontHit, wallDistance, wallLayer) && !isGrounded)
    //    {
    //        Debug.DrawLine(transform.position, transform.position + body.forward * wallDistance, Color.red, 0.2f);
    //        if (!isClimbing)
    //        {
    //            rb.velocity = Vector3.zero;
    //            startPosition = transform.position;
    //            targetPosition = wallFrontHit.point + (wallFrontHit.normal * offSetFromWall);
    //            climbingRotation = -wallFrontHit.normal;
    //            animatorManager.PlayTargetAnimation("Ledge_Idle", false, 0.1f);
    //            isClimbing = true;
    //        }
    //    }
    //    else
    //        isClimbing = false;

    //    if (isClimbing)
    //    {
    //        if (!isLerping)
    //        {
    //            moveDirection = body.up * inputManager.verticalInput;
    //            moveDirection += body.right * inputManager.horizontalInput;
    //            moveDirection.Normalize();

    //            if (isMid)
    //            {
    //                if (moveDirection == Vector3.zero)
    //                    return;
    //            }
    //            else
    //            {
    //                if (!ClimbDirection(moveDirection) || moveDirection == Vector3.zero)
    //                    return;
    //            }

    //            isMid = !isMid;

    //            t = 0;
    //            isLerping = true;
    //            startPosition = transform.position;
    //            targetPosition = (isMid) ? Vector3.Lerp(startPosition, rayCastHitPosition, 0.5f) : rayCastHitPosition;

    //        }
    //        else
    //        {
    //            t += Time.deltaTime * climbingSpeed;
    //            if (t > 1)
    //            {
    //                t = 1;
    //                isLerping = false;
    //            }

    //            //rb.MovePosition(Vector3.Lerp(startPosition, targetPosition, t));
    //            transform.position = Vector3.Lerp(transform.position, targetPosition, t);

    //        }
    //    }
    //}
}
