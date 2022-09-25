using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : BaseStats
{
    BaseStatusBar hpBar;
    PlayerAnimatorManager anim;
    public float weaponpower;

    private void Awake()
    {
        anim = GetComponent<PlayerAnimatorManager>();
        hpBar = GetComponentInChildren<BaseStatusBar>();
        //hpBar.UpdateMaxSliderValue();
        elementalTrahshold = elementalTrahsholdBase;
        SetCurrentValueToMaxValue();
    }

    public override BaseStats TakeDamage(float amount, ElementsEnum elementalDamageType)
    {
        amount /= GetStatByEnum(StatEnum.Defense).statvalues.currentValue;
        if (GetStatByEnum(StatEnum.HealthPoints).statvalues.ChangeCurrentStat(-amount) <= 0)
        {
            return this;
        }

        if ((elementalTrahshold -= amount) < 0)
        {
            switch(elementalDamageType)
            {
                case ElementsEnum.Fire:
                    GetStatByEnum(StatEnum.Stamina).statvalues.ChangeStatLevel(-1);
                    break;
                case ElementsEnum.Earth:
                    GetStatByEnum(StatEnum.Defense).statvalues.ChangeStatLevel(-1);
                    break;
            }
            elementalTrahsholdBase *= 2;
            elementalTrahshold = elementalTrahsholdBase;
        }
        return null;
    }

    public override void DoDamage(BaseStats targetHit)
    {
        targetHit.TakeDamage(weaponpower*GetStatByEnum(StatEnum.Attack).statvalues.currentValue,ElementsEnum.Fire);
    }
}
