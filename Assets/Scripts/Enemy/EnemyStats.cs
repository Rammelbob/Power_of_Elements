using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : BaseStats
{
    BaseStatusBar hpBar;
    PlayerAnimatorManager anim;

    private void Awake()
    {
        anim = GetComponent<PlayerAnimatorManager>();
        hpBar = GetComponentInChildren<BaseStatusBar>();
        //hpBar.UpdateMaxSliderValue();
        SetCurrentValueToMaxValue();
    }

    public override void TakeDamage(float amount,DamageCollider damageCollider)
    {
        //damage calc Switch case für den elemental multiplier und def

        //healthPoints.LoseStat(amount, 0, hpBar.UpdateSliderValue);
        //anim.PlayPlayerTargetAnimation("Take Damage", true, 0.1f, true, true);
        //if (healthPoints.currentValue == 0)
        //    anim.PlayPlayerTargetAnimation("Death", true, 0.1f, true, true);

    }
}
