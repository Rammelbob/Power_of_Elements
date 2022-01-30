using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : BaseStats
{
    AnimatorManager animatorManager;

    [Header("HealthPoints")]
    public int extraHPLvl;
    public float extraHpPerLvl;
    public AdvancedStatusBar AdvancedHPBar;

    [Header("Stamina")]
    public float baseStamina;
    public float currentStamina;
    public int extraStaminaLvl;
    public float extraStaminaPerLvl;
    public AdvancedStatusBar AdvancedStaminaBar;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        currentHP = GetMaxHP();
    }

    protected override float GetMaxHP()
    {
        return baseHP + extraHpPerLvl * extraHPLvl;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            DamageCalculation(gameObject);
    }
}
