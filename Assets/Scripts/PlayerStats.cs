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

    private float GetMaxHP()
    {
        return baseHP + extraHpPerLvl * extraHPLvl;
    }

    public override void TakeDamage(float damage)
    {
        currentHP -= damage;
        AdvancedHPBar.UpdateSliderValue(currentHP);

        animatorManager.PlayTargetAnimation("Take Damage", true, 0.1f, true, true);
        if (currentHP <= 0)
        {
            currentHP = 0;
            animatorManager.PlayTargetAnimation("Death", true, 0.1f, true, true);
        }
    }

    public override void GainHP(float amount)
    {
        base.GainHP(amount);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            TakeDamage(25);
    }
}
