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

    float timeOfLastHit;
    public float timeAfterHitToHeal;

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

    public void RestoreStats()
    {
        if (!GetStatByEnum(StatEnum.Stamina).statvalues.IsCurrentMax())
        {
            GetStatByEnum(StatEnum.Stamina).statvalues.ChangeCurrentStat(regenPerSec * Time.deltaTime);
        }

        if (!GetStatByEnum(StatEnum.HealthPoints).statvalues.IsCurrentMax())
        {
            if (timeOfLastHit < Time.time - timeAfterHitToHeal)
            {
                GetStatByEnum(StatEnum.HealthPoints).statvalues.ChangeCurrentStat(regenPerSec * Time.deltaTime);
            }
        }
    }

    public override BaseStats TakeDamage(float amount, ElementsEnum elementalDamageType)
    {
        amount /= GetStatByEnum(StatEnum.Defense).statvalues.currentValue;
        //GetStatByEnum(StatEnum.HealthPoints).statvalues.ChangeCurrentStat(-amount);
        timeOfLastHit = Time.time;
        if (GetStatByEnum(StatEnum.HealthPoints).statvalues.ChangeCurrentStat(-amount) <= 0)
        {
            return this;
        }
        return null;
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
