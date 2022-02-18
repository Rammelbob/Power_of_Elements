using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerLocomotion playerLocomotion;
    PlayerInventory playerInventory;
    public Animator animator;

    [Header("Player Flags")]
    public bool isInteracting;
    public bool useRootMotion;
    public bool canDoCombo;
    public bool canRotate;
    public bool isBlocking;
    public bool unLoadWeapons;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerInventory = GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();

        canDoCombo = animator.GetBool("canDoCombo");
        if (unLoadWeapons)
        {
            playerInventory.UnLoadWeapons();
            animator.SetBool("unLoadWeapon", false);
        }
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
        unLoadWeapons = animator.GetBool("unLoadWeapon");
    }
}
