using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    public WeaponHolderSlot leftHandSlot;
    public WeaponHolderSlot rightHandSlot;

    ShieldCollider leftHandCollider;
    DamageCollider rightHandCollider;

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

    public void LoadWeaponOnSlot(bool isLeft, Weapon weapon = null)
    {
        if (isLeft)
        {
            leftHandSlot.LoadWeaponModel(weapon);
            LoadLeftWeaponDamageCollider();
        }
        else
        {
            rightHandSlot.LoadWeaponModel(weapon);
            LoadRightWeaponDamageCollider();
        }
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

    public void OpenLeftDamageCollider()
    {
        leftHandCollider.EnableShieldCollider();
    }

    public void OpenRightDamageCollider()
    {
        rightHandCollider.EnableDamageCollider();
    }

    public void CloseLeftDamageCollider()
    {
        leftHandCollider.DisableShieldCollider();
    }

    public void CloseRightDamageCollider()
    {
        rightHandCollider.DisableDamageCollider();
    }
    #endregion
}
