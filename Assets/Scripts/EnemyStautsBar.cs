using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStautsBar : MonoBehaviour
{
    public Slider hpSlider;

    public void UpdateHpBar(float value)
    {
        hpSlider.value = value;
    }

    public void UpdateHpMaxValue(float value)
    {
        hpSlider.maxValue = value;
    }
}
