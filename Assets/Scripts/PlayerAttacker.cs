using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    AnimatorManager animatorManager;
    InputManager inputManager;
    public string lastAttack;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
    }

    public void HandleWeaponCombo(Weapon weapon)
    {
        if (inputManager.comboFlag)
        {
            animatorManager.animator.SetBool("canDoCombo", false);
            if (lastAttack == weapon.light_Attack_1)
            {
                animatorManager.PlayTargetAnimation(weapon.light_Attack_2, true, 0.1f, true, true);
                lastAttack = weapon.light_Attack_2;
            }
            else if (lastAttack == weapon.light_Attack_2)
            {
                animatorManager.PlayTargetAnimation(weapon.light_Attack_3, true, 0.1f, true, true);
                lastAttack = weapon.light_Attack_3;
            }  
            else if (lastAttack == weapon.light_Attack_3)
            {
                animatorManager.PlayTargetAnimation(weapon.light_Attack_4, true, 0.1f, true, true);
            }
            else if (lastAttack == weapon.heavy_Attack_1)
            {
                animatorManager.PlayTargetAnimation(weapon.heavy_Attack_2, true, 0.1f, true, true);
            }
        }
    }

    public void HandleLightAttack(Weapon weapon)
    {
        animatorManager.PlayTargetAnimation(weapon.light_Attack_1, true, 0.1f, true, true);
        lastAttack = weapon.light_Attack_1;
    }
    
    public void HandleHeavyAttack(Weapon weapon)
    {
        animatorManager.PlayTargetAnimation(weapon.heavy_Attack_1, true, 0.1f, true, true);
        lastAttack = weapon.heavy_Attack_1;
    }
}
