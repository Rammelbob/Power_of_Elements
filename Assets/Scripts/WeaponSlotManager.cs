using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WeaponSlotManager : MonoBehaviour
{
    public WeaponHolderSlot leftHandSlot;
    public WeaponHolderSlot rightHandSlot;

    CombatCollider leftHandCollider;
    CombatCollider rightHandCollider;

    VisualEffect combatVFX;


    private void Awake()
    {
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot.isLeftHandSlot)
            {
                leftHandSlot = weaponSlot;
            }
            else if (weaponSlot.isRightHandSlot)
            {
                rightHandSlot = weaponSlot;
            }
        }
    }

    public void LoadNewWeaponOnSlot(bool isLeft, Weapon weapon)
    {
        if (isLeft)
        {
            leftHandSlot.LoadNewWeaponModel(weapon.leftHandWeapon);
            LoadLeftWeaponDamageCollider();
        }
        else
        {
            rightHandSlot.LoadNewWeaponModel(weapon.rightHandWeapon);
            LoadRightWeaponDamageCollider();
            LoadWeaponVFX();
        }
    }

    public void ShowCurrentWeapon(bool isLeft, bool showWeapon)
    {
        if (isLeft)
            leftHandSlot.ShowCurrentWeapon(showWeapon);
        else
            rightHandSlot.ShowCurrentWeapon(showWeapon);
    }


    #region Handle Weapons Collider
    private void LoadLeftWeaponDamageCollider()
    {
        leftHandCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<ShieldCollider>();
    }

    private void LoadRightWeaponDamageCollider()
    {
        rightHandCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    private void LoadWeaponVFX()
    {
        combatVFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<VisualEffect>();
    }

    public void OpenLeftDamageCollider()
    {
        if (leftHandCollider)
            leftHandCollider.EnableCombatCollider();
    }

    public void OpenRightDamageCollider()
    {
        if (rightHandCollider)
            rightHandCollider.EnableCombatCollider();

        PlayVFX();
    }

    public void CloseLeftDamageCollider()
    {
        if (leftHandCollider)
            leftHandCollider.DisableCombatCollider();
    }

    public void CloseRightDamageCollider()
    {
        if (rightHandCollider)
            rightHandCollider.DisableCombatCollider();

        StopVFX();
    }

    private void PlayVFX()
    {
        combatVFX.Play();
    }

    private void StopVFX()
    {
        combatVFX.Stop();
    }
    #endregion
}
