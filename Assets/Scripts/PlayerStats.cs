using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : BaseStats
{
    AnimatorManager animatorManager;
    PlayerAttacker playerAttacker;
    WeaponSlotManager weaponSlotManager;

    [Header("HealthPoints")]
    public int extraHPLvl;
    public float extraHpPerLvl;
    public AdvancedStatusBar AdvancedHPBar;

    [Header("Stamina")]
    public float baseStamina;
    public float currentStamina;
    public int extraStaminaLvl;
    public float extraStaminaPerLvl;
    public AdvancedStatusBar AdvancedStaminaBar;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerAttacker = GetComponent<PlayerAttacker>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        currentHP = GetMaxHP();
    }

    protected override float GetMaxHP()
    {
        return baseHP + extraHpPerLvl * extraHPLvl;
    }

    public override void TakeDamage(float amount,DamageCollider damageCollider)
    {
        //damage calc Switch case für den elemental multiplier und def
       
        //foreach (DamageCollider item in blockedColliders)
        //{
        //    if (item == damageCollider)
        //    {
        //        //weapon.leftHandWeapon.shiledHP -= amount;
        //        //if (weapon.leftHandWeapon.shiledHP >= 0)
        //        //{
        //        //    weaponSlotManager.leftHandSlot.UnloadWeaponAndDestroy();
        //        //}
        //        //else
        //        //{
        //        //    amount = 0;
        //        //}
                
        //        //blockedColliders.Remove(item);
        //        //break;
        //    }
        //}
        LoseStat(ref currentHP, amount, 0, AdvancedHPBar.UpdateSliderValue);
    }
}
