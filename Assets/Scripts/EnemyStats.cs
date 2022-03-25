using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : BaseStats
{
    BaseStatusBar hpBar;
    AnimatorManager anim;

    private void Awake()
    {
        anim = GetComponent<AnimatorManager>();
        hpBar = GetComponentInChildren<BaseStatusBar>();
        hpBar.UpdateMaxSliderValue(baseHP);
        currentHP = baseHP;
    }

    public override void TakeDamage(float amount,DamageCollider damageCollider)
    {
        //damage calc Switch case für den elemental multiplier und def

        LoseStat(ref currentHP, amount, 0, hpBar.UpdateSliderValue);
        anim.PlayTargetAnimation("Take Damage", true, 0.1f, true, true);
        if (currentHP == 0)
            anim.PlayTargetAnimation("Death", true, 0.1f, true, true);
    }
}
