using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour,ICombat
{
    PlayerManager playerManager;
    public PlayerAttack currentAttack;
    public bool doWeaponCombo;
    public bool comboLight;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    public void HandleWeaponCombo()
    {
        playerManager.playerAnimatorManager.animator.SetBool("canDoCombo", false);
        if (doWeaponCombo)
        {
            doWeaponCombo = false;
            if (comboLight)
            {
                if (currentAttack.nextLightAttack != null)
                {
                    playerManager.playerAnimatorManager.PlayPlayerTargetAnimation(currentAttack.nextLightAttack.attackName, true, 0.01f, true, true);
                    currentAttack = currentAttack.nextLightAttack;
                    playerManager.playerInventory.ShowCurrentWeapon(true);
                }
            }
            else                                                                                   
            {
                if (currentAttack.nextHeavyAttack != null)
                {
                    playerManager.playerAnimatorManager.PlayPlayerTargetAnimation(currentAttack.nextHeavyAttack.attackName, true, 0.01f, true, true);
                    currentAttack = currentAttack.nextHeavyAttack;
                    playerManager.playerInventory.ShowCurrentWeapon(true);
                }
            }
        }
    }

    public void HandleLightAttack(PlayerWeaponItem weapon)
    {
        if (weapon.first_Light_Attack != null)
        {
            playerManager.playerAnimatorManager.PlayPlayerTargetAnimation(weapon.first_Light_Attack.attackName, true, 0.1f, true, true);
            currentAttack = weapon.first_Light_Attack;
            playerManager.playerInventory.ShowCurrentWeapon(true);
        }
    }
    
    public void HandleHeavyAttack(PlayerWeaponItem weapon)
    {
        if (weapon.first_Heavy_Attack != null)
        {
            playerManager.playerInventory.ShowCurrentWeapon(true);
            playerManager.playerAnimatorManager.PlayPlayerTargetAnimation(weapon.first_Heavy_Attack.attackName, true, 0.1f, true, true);
            currentAttack = weapon.first_Heavy_Attack;
        }
    }

    public void HandleRunningAttack(PlayerWeaponItem weapon)
    {
        if (weapon.running_Attack != null)
        {
            playerManager.playerInventory.ShowCurrentWeapon(true);
            playerManager.playerAnimatorManager.PlayPlayerTargetAnimation(weapon.running_Attack.attackName, true, 0.1f, true, true);
            currentAttack = weapon.running_Attack;
        }
    }

    public void StartBlock()
    {
        if (!playerManager.isBlocking)
        {
            playerManager.playerInventory.ShowCurrentWeapon(true);
            playerManager.playerAnimatorManager.PlayPlayerTargetAnimation("Shield_Block_Idle", true, 0.2f, false, true);
        }   
    }

    public void DoAttack(List<GameObject> hitList)
    {
        throw new System.NotImplementedException();
    }

    public void GetAttacked(float damage, ElementsEnum damageType, bool isStagger)
    {
        throw new System.NotImplementedException();
    }
}
