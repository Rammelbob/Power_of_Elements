using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : BaseStats
{
    public BaseStatusBar hpBar;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        hpBar.UpdateMaxSliderValue(baseHP);
        currentHP = baseHP;
    }
}
