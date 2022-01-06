using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : BaseStats
{
    public BaseStatusBar hpBar;

    private void Awake()
    {
        hpBar.UpdateMaxSliderValue(baseHP);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        hpBar.UpdateSliderValue(currentHP);
    }

    public override void GainHP(float amount)
    {
        base.GainHP(amount);
        hpBar.UpdateSliderValue(currentHP);
    }

}
