using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseStatusBar : MonoBehaviour
{
    public Slider baseSlider;

    public void UpdateSliderValue(float value)
    {
        if (baseSlider.maxValue >= value)
            baseSlider.value = value;
        else
            baseSlider.value = baseSlider.maxValue;
    }

    public virtual void UpdateMaxSliderValue(float maxValue)
    {
        baseSlider.maxValue = maxValue;
        baseSlider.value = baseSlider.maxValue;
    }
}