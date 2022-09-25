using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public abstract class BaseStats : MonoBehaviour
{
    [Header("Stats")]

    public Stat[] statsArray = new Stat[Enum.GetValues(typeof(StatEnum)).Length];
    public int id;
    public float elementalTrahshold;
    protected float elementalTrahsholdBase = 50;


    public abstract BaseStats TakeDamage(float amount,ElementsEnum elementalDamageType);
    public abstract void DoDamage(BaseStats targetHit);

    public void SetCurrentValueToMaxValue()
    {
        //statsArray = statsArray.OrderBy(i => i.statType).ToArray();
        for (int i = 0; i < statsArray.Length; i++)
        {
            statsArray[i].statvalues.SetCurrentValueToMaxValue();
        }
    }

    public Stat GetStatByEnum(StatEnum statEnum)
    {
        return statsArray.Where(i => i.statType == statEnum).First();
    }
}

[Serializable]
public class StatValues
{
    public float baseValue;
    public float currentValue;
    public float amountPerLevel;
    public int statLevel;
    int statLevleMinMax = 10;

    public event Action<float> OnStatChange;
    public event Action<int,float> OnLevelChange;

    public void SetCurrentValueToMaxValue()
    {
        currentValue = GetMaxValue();
        OnStatChange?.Invoke(currentValue);
    }

    public float GetMaxValue()
    {
        return baseValue + statLevel * amountPerLevel;
    }

    public float ChangeCurrentStat(float changeAmount)
    {
        currentValue += changeAmount;
        currentValue = Mathf.Clamp(currentValue, 0, GetMaxValue());

        OnStatChange?.Invoke(currentValue);
        return currentValue;
    }

    public int ChangeStatLevel(int changeAmount)
    {
        float currentValueinPercent = currentValue / GetMaxValue();
        statLevel += changeAmount;
        statLevel = Mathf.Clamp(statLevel, -statLevleMinMax, statLevleMinMax);

        OnLevelChange?.Invoke(statLevel, GetMaxValue());
        ChangeCurrentStat(currentValueinPercent * GetMaxValue() - currentValue);

        return statLevel;
    }
}

[Serializable]
public class Stat
{
    public StatEnum statType;
    public StatValues statvalues;
}



