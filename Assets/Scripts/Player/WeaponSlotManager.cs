using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WeaponSlotManager : MonoBehaviour
{
    public WeaponHolderSlot rightHandSlot;

    CombatCollider rightHandCollider;

    GameObject combatVFXBuffer;
    GameObject combatVFX;


    private void Awake()
    {
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot.isRightHandSlot)
            {
                rightHandSlot = weaponSlot;
            }
        }
    }

    public void LoadNewWeaponOnSlot(PlayerWeaponItem weapon)
    {

        rightHandSlot.LoadNewWeaponModel(weapon.rightHandWeapon);
        LoadRightWeaponDamageCollider();
        LoadWeaponVFX(weapon.attackVisualEffect);
    }

    public void ShowCurrentWeapon(bool showWeapon)
    {
        rightHandSlot.ShowCurrentWeapon(showWeapon);
    }


    #region Handle Weapons Collider
    private void LoadRightWeaponDamageCollider()
    {
        rightHandCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    private void LoadWeaponVFX(GameObject visualEffect)
    {
        combatVFXBuffer = visualEffect;
    }

    public void OpenRightDamageCollider()
    {
        if (rightHandCollider)
            rightHandCollider.EnableCombatCollider();

        PlayVFX();
    }

    public void CloseRightDamageCollider()
    {
        if (rightHandCollider)
            rightHandCollider.DisableCombatCollider();

        StopVFX();
    }

    private void PlayVFX()
    {
        if (combatVFXBuffer)
        {
            Transform weaponPivot = rightHandSlot.currentWeaponModel.transform.Find("Weapon Pivot");
            if (weaponPivot)
            {
                combatVFX = Instantiate(combatVFXBuffer, weaponPivot.position, weaponPivot.rotation);
            }
        }
    }

    private void StopVFX()
    {
        Destroy(combatVFX);
    }
    #endregion
}
