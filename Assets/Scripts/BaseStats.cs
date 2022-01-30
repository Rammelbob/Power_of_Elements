using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseStats : MonoBehaviour
{
    public float baseHP;
    public float currentHP;

    public float defense;
    public Element element;

    float currentWeaponDamage;


    public void SetCurrentWeaponDamage(float weaponDamage)
    {
        currentWeaponDamage = weaponDamage;
    }

    protected virtual float GetMaxHP()
    {
        return baseHP;
    }

    public void GainStat(ref float currentAmount, float gainAmount, float maxAmount, Action<float> changeSliderValue)
    {
        gainAmount = Mathf.Abs(gainAmount);
        currentAmount = currentAmount + gainAmount > maxAmount ? maxAmount : currentAmount + gainAmount;
        changeSliderValue(currentAmount);
    }

    public void LoseStat(ref float currentAmount, float loseAmount, float minAmount, Action<float> changeSliderValue)
    {
        loseAmount = Mathf.Abs(loseAmount);
        currentAmount = currentAmount - loseAmount < minAmount ? minAmount : currentAmount - loseAmount;
        changeSliderValue(currentAmount);
    }

    public void TakeDamage(float amount)
    {
        LoseStat(ref currentHP, amount, 0, GetComponentInChildren<BaseStatusBar>().UpdateSliderValue);
    }

    public void DamageCalculation(GameObject hit)
    {
        BaseStats hitBaseStats = hit.GetComponentInParent<BaseStats>();
        if (hitBaseStats == null)
            return;

        // damage Calc

        hitBaseStats.TakeDamage(currentWeaponDamage);
    }
}


