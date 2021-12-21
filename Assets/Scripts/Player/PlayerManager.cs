using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerLocomotion playerLocomotion;
    public Animator animator;

    [Header("Player Flags")]
    public bool isInteracting;
    public bool useRootMotion;
    public bool canDoCombo;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();

        canDoCombo = animator.GetBool("canDoCombo");
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
        animator.SetFloat("inAirTime", playerLocomotion.inAirTimer);
        animator.SetFloat("distanceToGround", playerLocomotion.distanceToGround);
        animator.SetBool("isGrounded", playerLocomotion.isGrounded);
    }

    private void LateUpdate()
    {
        inputManager.lightAttack = false;
        inputManager.heavyAttack = false;


        isInteracting = animator.GetBool("isInteracting");
        playerLocomotion.isJumping = animator.GetBool("isJumping");
        useRootMotion = animator.GetBool("useRootMotion");
    }
}
