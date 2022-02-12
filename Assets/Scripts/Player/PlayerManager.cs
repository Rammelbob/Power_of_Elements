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
    public bool canRotate;
    public bool isBlocking;

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
        animator.SetBool("isGrounded", playerLocomotion.isGrounded);
        animator.SetBool("isBlocking", inputManager.blockInput);
    }

    private void LateUpdate()
    {
        inputManager.lightAttack = false;
        inputManager.heavyAttack = false;

        canRotate = animator.GetBool("canRotate");
        isInteracting = animator.GetBool("isInteracting");
        playerLocomotion.isJumping = animator.GetBool("isJumping");
        useRootMotion = animator.GetBool("useRootMotion");
        isBlocking = animator.GetBool("isBlocking");
    }
}
