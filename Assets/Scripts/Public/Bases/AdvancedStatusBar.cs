using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdvancedStatusBar : BaseStatusBar
{
    public Slider followSlider;
    public RectTransform sliderRect;
    public float offSetMaxPerLvl;


    private void Awake()
    {
        if (followSlider == null)
            followSlider = baseSlider;
    }

    public void SetSliderSizeWithLvl(int level,float sliderMaxValue)
    {
        UpdateMaxSliderValue(sliderMaxValue);
        sliderRect.offsetMax = new Vector2(level * offSetMaxPerLvl, sliderRect.offsetMax.y);
    }

    public void LerpFollowSlider(float delta)
    {
        if (Mathf.Abs(followSlider.value - baseSlider.value) > 0.1)
        {
            followSlider.value = Mathf.Lerp(followSlider.value, baseSlider.value, delta);
        }
        else
            followSlider.value = baseSlider.value;
    }

    public override void UpdateMaxSliderValue(float maxValue)
    {
        base.UpdateMaxSliderValue(maxValue);
        followSlider.maxValue = maxValue;
    }

    private void Update()
    {
        LerpFollowSlider(Time.deltaTime);
    }
}
