using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Stat : MonoBehaviour
{
    public TextMeshProUGUI statTypeText;
    public TextMeshProUGUI statLvlText;
    public StatEnum statType;

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

}
