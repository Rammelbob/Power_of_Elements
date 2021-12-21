using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    WeaponSlotManager weaponSlotManager;

    public Weapon rightWeapon;
    public Weapon leftWeapon;

    private void Awake()
    {
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }

    private void Start()
    {
        if (rightWeapon)
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        if(leftWeapon)
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
    }
}
