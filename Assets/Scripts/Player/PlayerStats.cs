using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : BaseStats
{
    [Header("Status Bars")]
    public AdvancedStatusBar advancedHPBar;
    public AdvancedStatusBar advancedStaminaBar;

    PlayerManager playerManager;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        GetStatByEnum(StatEnum.HealthPoints).statvalues.OnStatChange += OnHealthPointChange;
        GetStatByEnum(StatEnum.HealthPoints).statvalues.OnLevelChange += OnHealthPointLevelChange;

        GetStatByEnum(StatEnum.Stamina).statvalues.OnStatChange += OnStaminaChange;
        GetStatByEnum(StatEnum.Stamina).statvalues.OnLevelChange += OnStaminaLevelChange;

        GetStatByEnum(StatEnum.MovementSpeed).statvalues.OnStatChange += OnMovementSpeedChange;
        GetStatByEnum(StatEnum.AttackSpeed).statvalues.OnStatChange += OnAttackSpeedChange;

        advancedHPBar.UpdateMaxSliderValue(GetStatByEnum(StatEnum.HealthPoints).statvalues.GetMaxValue());
        advancedStaminaBar.UpdateMaxSliderValue(GetStatByEnum(StatEnum.Stamina).statvalues.GetMaxValue());
        SetCurrentValueToMaxValue();
    }

    private void OnDisable()
    {
        GetStatByEnum(StatEnum.HealthPoints).statvalues.OnStatChange -= OnHealthPointChange;
        GetStatByEnum(StatEnum.HealthPoints).statvalues.OnLevelChange -= OnHealthPointLevelChange;

        GetStatByEnum(StatEnum.Stamina).statvalues.OnStatChange -= OnStaminaChange;
        GetStatByEnum(StatEnum.Stamina).statvalues.OnLevelChange -= OnStaminaLevelChange;

        GetStatByEnum(StatEnum.MovementSpeed).statvalues.OnStatChange -= OnMovementSpeedChange;
        GetStatByEnum(StatEnum.AttackSpeed).statvalues.OnStatChange -= OnAttackSpeedChange;
    }

    private void OnHealthPointChange(float currentAmount)
    {
        advancedHPBar.UpdateSliderValue(currentAmount);
    }

    private void OnHealthPointLevelChange(int level, float maxAmount)
    {
        advancedHPBar.SetSliderSizeWithLvl(level, maxAmount);
    }

    private void OnStaminaChange(float currentAmount)
    {
        advancedStaminaBar.UpdateSliderValue(currentAmount);
    }

    private void OnStaminaLevelChange(int level, float maxAmount)
    {
        advancedStaminaBar.SetSliderSizeWithLvl(level, maxAmount);
    }
    private void OnAttackSpeedChange(float currentAmount)
    {
        playerManager.playerAnimatorManager.animator.SetFloat("attackSpeedMultiplier", currentAmount);
    }

    private void OnMovementSpeedChange(float currentAmount)
    {
        playerManager.playerAnimatorManager.animator.SetFloat("movementSpeedMultiplier", currentAmount);
    }

    public override void TakeDamage(float amount,DamageCollider damageCollider)
    {
        //damage calc Switch case für den elemental multiplier und def
       
        //foreach (DamageCollider item in blockedColliders)
        //{
        //    if (item == damageCollider)
        //    {
        //        //weapon.leftHandWeapon.shiledHP -= amount;
        //        //if (weapon.leftHandWeapon.shiledHP >= 0)
        //        //{
        //        //    weaponSlotManager.leftHandSlot.UnloadWeaponAndDestroy();
        //        //}
        //        //else
        //        //{
        //        //    amount = 0;
        //        //}
                
        //        //blockedColliders.Remove(item);
        //        //break;
        //    }
        //}
        //healthPoints.LoseStat(amount, 0, advancedHPBar.UpdateSliderValue);
    }
}
