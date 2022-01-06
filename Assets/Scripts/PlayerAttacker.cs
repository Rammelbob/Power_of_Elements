using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    AnimatorManager animatorManager;
    InputManager inputManager;
    public Attack currentAttack;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
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
                    }
                }
                else
                {
                    if (currentAttack.nextHeavyAttack != null)
                    {
                        animatorManager.PlayTargetAnimation(currentAttack.nextHeavyAttack.attackName, true, 0.1f, true, true);
                        currentAttack = currentAttack.nextHeavyAttack;
                    }
                }
                
            }
        }
    }

    public void HandleLightAttack(Weapon weapon)
    {
        animatorManager.PlayTargetAnimation(weapon.first_Light_Attack.attackName, true, 0.1f, true, true);
        currentAttack = weapon.first_Light_Attack;
    }
    
    public void HandleHeavyAttack(Weapon weapon)
    {
        animatorManager.PlayTargetAnimation(weapon.first_Heavy_Attack.attackName, true, 0.1f, true, true);
        currentAttack = weapon.first_Heavy_Attack;
    }
}
