using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceWeapon : MonoBehaviour
{
    int pointsToUse = 0;

    public TMP_InputField InputField;

    public UI_Skillpoint_Handler SkillpointHandler;
    private Text textMeshProUGUI;
    public UI_WeaponSlot weaponSlot;

    void Start()
    {
        InputField.text = pointsToUse.ToString();
    }

    public void IncreasePointsToUse()
    {
        if (pointsToUse >= SkillpointHandler.SkillPoints) { return; }

        InputField.text = (++pointsToUse).ToString();
    }

    public void DecreasePointsToUse()
    {
        if (pointsToUse <= 0) { return; }
        InputField.text = (--pointsToUse).ToString();
    }


    public void EnhamceWeapon(TMP_InputField pointsUsed)
    {
        int damagePerSkillPoint = 10;
        int usedPoints = Convert.ToInt16(pointsUsed.text);

        weaponSlot.currentWeapon.first_Light_Attack.attackDamage += damagePerSkillPoint * usedPoints;

        SkillpointHandler.SkillPoints -= usedPoints;

        pointsToUse = 0;
        InputField.text = "0";

        Debug.Log(weaponSlot.currentWeapon.first_Light_Attack.attackDamage);
    }
}
