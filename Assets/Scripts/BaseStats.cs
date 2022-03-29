using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseStats : MonoBehaviour
{
    public float baseHP;
    public float currentHP;

    public float defense;
    public Weapon weapon;

    float currentWeaponDamage = 10;

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

    public abstract void TakeDamage(float amount, DamageCollider damageCollider);

    public float CurrentWeaponDamage()
    {
        return currentWeaponDamage;
    }
}


