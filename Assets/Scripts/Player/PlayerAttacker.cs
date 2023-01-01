using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
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
                    DoAttack(currentAttack.nextLightAttack);
                }
            }
            else                                                                                   
            {
                if (currentAttack.nextHeavyAttack != null)
                {
                    DoAttack(currentAttack.nextHeavyAttack);
                }
            }
        }
    }

    void DoAttack(PlayerAttack playerAttack)
    {
        if (playerManager.playerStats.CanUseStamina(playerAttack.staminaCost))
        {
            playerManager.playerAnimatorManager.PlayPlayerTargetAnimation(playerAttack.attackName, true, 0.0f, true, true);
            playerManager.unloadWeapons = false;
            playerManager.playerInventory.ShowCurrentWeapon(true);
            //playerManager.playerStats.GetStatByEnum(StatEnum.Stamina).statvalues.ChangeCurrentStat(-playerAttack.staminaCost);
            currentAttack = playerAttack;
        }
    }

    public void HandleLightAttack(PlayerWeaponItem weapon)
    {
        if (weapon.first_Light_Attack != null)
        {
            DoAttack(weapon.first_Light_Attack);
        }
    }
    
    public void HandleHeavyAttack(PlayerWeaponItem weapon)
    {
        if (weapon.first_Heavy_Attack != null)
        {
            DoAttack(weapon.first_Heavy_Attack);
        }
    }

    public void HandleRunningAttack(PlayerWeaponItem weapon)
    {
        if (weapon.running_Attack != null)
        {
            DoAttack(weapon.running_Attack);
        }
    }
}
