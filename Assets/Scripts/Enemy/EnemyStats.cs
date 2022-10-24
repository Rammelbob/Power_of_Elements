using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : BaseStats
{
    public BaseStatusBar hpBar;
    public BaseStatusBar staminaBar;
    public BaseStatusBar elementalBar;
    public EnemyStateManager enemyStateManager;
    public ElementsEnum damageType;
    PlayerAnimatorManager anim;

    private void Awake()
    {
        anim = GetComponent<PlayerAnimatorManager>();
        elementalTrahshold = elementalTrahsholdBase;
        elementalBar.UpdateMaxSliderValue(elementalTrahsholdBase);
        elementalBar.UpdateSliderValue(elementalTrahshold);

        GetStatByEnum(StatEnum.HealthPoints).statvalues.OnStatChange += hpBar.UpdateSliderValue;
        hpBar.UpdateMaxSliderValue(GetStatByEnum(StatEnum.HealthPoints).statvalues.GetCurrentMaxValue());

        GetStatByEnum(StatEnum.Stamina).statvalues.OnStatChange += staminaBar.UpdateSliderValue;
        staminaBar.UpdateMaxSliderValue(GetStatByEnum(StatEnum.Stamina).statvalues.GetCurrentMaxValue());

        SetCurrentValueToMaxValue();
    }

    private void OnDisable()
    {
        GetStatByEnum(StatEnum.HealthPoints).statvalues.OnStatChange -= hpBar.UpdateSliderValue;
        GetStatByEnum(StatEnum.Stamina).statvalues.OnStatChange -= staminaBar.UpdateSliderValue;
    }

    public override BaseStats TakeDamage(float amount, ElementsEnum elementalDamageType)
    {
        amount /= GetStatByEnum(StatEnum.Defense).statvalues.currentValue;
        GetStatByEnum(StatEnum.HealthPoints).statvalues.ChangeCurrentStat(-amount);

        if (GetStatByEnum(StatEnum.HealthPoints).statvalues.currentValue <= 0)
        {
            return this;
        }

        elementalTrahshold -= amount;
        if (elementalTrahshold < 0)
        {
            switch(elementalDamageType)
            {
                case ElementsEnum.Fire:
                    GetStatByEnum(StatEnum.Attack).statvalues.ChangeStatLevel(-1);
                    break;
                case ElementsEnum.Earth:
                    GetStatByEnum(StatEnum.Defense).statvalues.ChangeStatLevel(-1);
                    break;
            }
            elementalTrahsholdBase *= 2;
            elementalBar.UpdateMaxSliderValue(elementalTrahsholdBase);
            elementalTrahshold = elementalTrahsholdBase;
            
        }
        elementalBar.UpdateSliderValue(elementalTrahshold);
        return null;
    }

    public override void DoDamage(BaseStats targetHit)
    {
        targetHit.TakeDamage(enemyStateManager.combatState.currentAttack.attackDamage * GetStatByEnum(StatEnum.Attack).statvalues.currentValue, damageType);
    }
}
