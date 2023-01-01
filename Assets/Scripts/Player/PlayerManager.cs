using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public InputManager inputManager;
    public PlayerLocomotion playerLocomotion;
    public PlayerInventory playerInventory;
    public PlayerStats playerStats;
    public PlayerAnimatorManager playerAnimatorManager;
    public PlayerAttacker playerAttacker;
    public UI_Inventory_Handler ui_Inventory_Handler;
    public UI_Skillpoint_Handler ui_Skillpoint_Handler;

    [Header("Player Flags")]
    public bool isInteracting;
    public bool useRootMotion;
    public bool canDoCombo;
    public bool canRotate;
    public bool isBlocking;
    public bool unloadWeapons;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerInventory = GetComponent<PlayerInventory>();
        playerStats = GetComponent<PlayerStats>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerAttacker = GetComponent<PlayerAttacker>();
        ui_Inventory_Handler = GetComponentInChildren<UI_Inventory_Handler>();
        ui_Skillpoint_Handler = GetComponentInChildren<UI_Skillpoint_Handler>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
        canDoCombo = playerAnimatorManager.animator.GetBool("canDoCombo");

        playerStats.RestoreStats();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
        playerAnimatorManager.animator.SetBool("isGrounded", playerLocomotion.isGrounded);
    }

    private void LateUpdate()
    {
        inputManager.lightAttack = false;
        inputManager.heavyAttack = false;

        canRotate = playerAnimatorManager.animator.GetBool("canRotate");
        isInteracting = playerAnimatorManager.animator.GetBool("isInteracting");
        useRootMotion = playerAnimatorManager.animator.GetBool("useRootMotion");
        unloadWeapons = playerAnimatorManager.animator.GetBool("unLoadWeapon");

        if (unloadWeapons)
        {
            playerInventory.ShowCurrentWeapon(false);
            playerAnimatorManager.animator.SetBool("unLoadWeapon", false);
        }
    }
}
