using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarManager : MonoBehaviour
{
    public Slider hpSlider,staminaSlider, hpFollowSlider;
    public RectTransform hpBarRT, staminaBarRT;
    bool isLerping;
    float offSetMaxperLevel = 40;

    public void SetHPBar(int level,float maxValue)
    {
        hpSlider.maxValue = maxValue;
        hpFollowSlider.maxValue = maxValue;
        hpBarRT.offsetMax = new Vector2(level * offSetMaxperLevel, hpBarRT.offsetMax.y);
    }

    public void SetStaminaBar(int level, float maxValue)
    {
        staminaSlider.maxValue = maxValue;
        staminaBarRT.offsetMax = new Vector2(level * offSetMaxperLevel, staminaBarRT.offsetMax.y);
    }

    public void UpdateHpBar(float value, bool shouldLerp)
    {
        hpSlider.value = value;

        if (shouldLerp)
            isLerping = true;
        else
        {
            hpFollowSlider.value = hpSlider.value;
            isLerping = false;
        }
    }

    public void UpdateStaminaBar(float value)
    {
        staminaSlider.value = value;
    }

    private void Update()
    {
        if (isLerping)
            hpFollowSlider.value = Mathf.Lerp(hpFollowSlider.value, hpSlider.value, 1 * Time.deltaTime);
    }
}
