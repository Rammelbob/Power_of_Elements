using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    AnimatorManager animatorManager;
    InputManager inputManager;
    PlayerStats playerStats;
    public Attack currentAttack;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
        playerStats = GetComponent<PlayerStats>();
    }

    public void HandleWeaponCombo(bool isLightAttack)
    {
        if (inputManager.comboFlag)
        {
            animatorManager.animator.SetBool("canDoCombo", false);
            if (inputManager.comboFlag)
            {
                if (isLightAttack)
                {
                    if (currentAttack.nextLightAttack != null)
                    {
                        animatorManager.PlayTargetAnimation(currentAttack.nextLightAttack.attackName, true, 0.1f, true, true);
                        currentAttack = currentAttack.nextLightAttack;
                        playerStats.SetCurrentWeaponDamage(currentAttack.attackDamage);
                    }
                }
                else
                {
                    if (currentAttack.nextHeavyAttack != null)
                    {
                        animatorManager.PlayTargetAnimation(currentAttack.nextHeavyAttack.attackName, true, 0.1f, true, true);
                        currentAttack = currentAttack.nextHeavyAttack;
                        playerStats.SetCurrentWeaponDamage(currentAttack.attackDamage);
                    }
                }
                
            }
        }
    }

    public void HandleLightAttack(Weapon weapon)
    {
        if (weapon.first_Light_Attack.attackName != null)
        {
            animatorManager.PlayTargetAnimation(weapon.first_Light_Attack.attackName, true, 0.1f, true, true);
            currentAttack = weapon.first_Light_Attack;
            playerStats.SetCurrentWeaponDamage(currentAttack.attackDamage);
        }
    }
    
    public void HandleHeavyAttack(Weapon weapon)
    {
        if(weapon.first_Heavy_Attack.attackName != null)
        {
            animatorManager.PlayTargetAnimation(weapon.first_Heavy_Attack.attackName, true, 0.1f, true, true);
            currentAttack = weapon.first_Heavy_Attack;
            playerStats.SetCurrentWeaponDamage(currentAttack.attackDamage);
        }
    }

    public void HandleRunningAttack(Weapon weapon)
    {
        if (weapon.running_Attack != null)
        {
            animatorManager.PlayTargetAnimation(weapon.running_Attack.attackName, true, 0.1f, true, true);
            currentAttack = weapon.running_Attack;
        }
    }
}
