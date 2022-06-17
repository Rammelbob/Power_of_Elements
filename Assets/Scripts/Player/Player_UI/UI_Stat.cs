using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class UI_Stat : MonoBehaviour, ISelectHandler
{
    public TextMeshProUGUI statTypeText;
    public TextMeshProUGUI statLvlText;
    public StatEnum statType;

    public event Action<RectTransform> OnUIStatSelect;

    public void SetStat(StatEnum stat,int statLvl)
    {
        statType = stat;
        statLvlText.text = statLvl.ToString();
        statTypeText.text = statType.ToString();
    }

    public void SetLvl(int statLvl)
    {
        statLvlText.text = statLvl.ToString();
    }

    public void OnSelect(BaseEventData eventData)
    {
        OnUIStatSelect?.Invoke(GetComponent<RectTransform>());
    }
}
