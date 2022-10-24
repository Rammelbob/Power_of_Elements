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
    public static event Action<int> kill;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        GetStatByEnum(StatEnum.HealthPoints).statvalues.OnStatChange += advancedHPBar.UpdateSliderValue;
        GetStatByEnum(StatEnum.HealthPoints).statvalues.OnLevelChange += advancedHPBar.SetSliderSizeWithLvl;

        GetStatByEnum(StatEnum.Stamina).statvalues.OnStatChange += advancedStaminaBar.UpdateSliderValue;
        GetStatByEnum(StatEnum.Stamina).statvalues.OnLevelChange += advancedStaminaBar.SetSliderSizeWithLvl;

        GetStatByEnum(StatEnum.MovementSpeed).statvalues.OnStatChange += OnMovementSpeedChange;
        GetStatByEnum(StatEnum.AttackSpeed).statvalues.OnStatChange += OnAttackSpeedChange;

        advancedHPBar.UpdateMaxSliderValue(GetStatByEnum(StatEnum.HealthPoints).statvalues.GetCurrentMaxValue());
        advancedStaminaBar.UpdateMaxSliderValue(GetStatByEnum(StatEnum.Stamina).statvalues.GetCurrentMaxValue());

        elementalTrahshold = elementalTrahsholdBase;
        SetCurrentValueToMaxValue();
    }

    private void OnDisable()
    {
        GetStatByEnum(StatEnum.HealthPoints).statvalues.OnStatChange -= advancedHPBar.UpdateSliderValue;
        GetStatByEnum(StatEnum.HealthPoints).statvalues.OnLevelChange -= advancedHPBar.SetSliderSizeWithLvl;

        GetStatByEnum(StatEnum.Stamina).statvalues.OnStatChange -= advancedStaminaBar.UpdateSliderValue;
        GetStatByEnum(StatEnum.Stamina).statvalues.OnLevelChange -= advancedStaminaBar.SetSliderSizeWithLvl;

        GetStatByEnum(StatEnum.MovementSpeed).statvalues.OnStatChange -= OnMovementSpeedChange;
        GetStatByEnum(StatEnum.AttackSpeed).statvalues.OnStatChange -= OnAttackSpeedChange;
    }

    private void OnAttackSpeedChange(float currentAmount)
    {
        playerManager.playerAnimatorManager.animator.SetFloat("attackSpeedMultiplier", currentAmount);
    }

    private void OnMovementSpeedChange(float currentAmount)
    {
        playerManager.playerAnimatorManager.animator.SetFloat("movementSpeedMultiplier", currentAmount);
    }

    public override BaseStats TakeDamage(float amount, ElementsEnum elementalDamageType)
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
        return this;
    }

    public override void DoDamage(BaseStats targetHit)
    {
        var temp = targetHit.TakeDamage(playerManager.playerAttacker.currentAttack.attackDamage * GetStatByEnum(StatEnum.Attack).statvalues.currentValue,playerManager.playerInventory.currentWeapon.element);
        if (temp != null)
        {
            kill?.Invoke(temp.id);
            Destroy(temp.gameObject);
        } 
    }
}
