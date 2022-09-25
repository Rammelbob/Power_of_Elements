using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_WeaponSlot : UI_ItemSlot
{
    public event Action<PlayerWeaponItem> UnequipWeapon;
    public PlayerWeaponItem currentWeapon;


    public override void UnEquip(UI_Item itemToUnequip, UI_ItemSlot newParnetSlot)
    {
        if (newParnetSlot.GetSlotType() != ItemTypeEnum.Weapon)
        {
            currentWeapon = (PlayerWeaponItem)itemToUnequip.itemInfo;
            UnequipWeapon?.Invoke(currentWeapon);
        }
        currentWeapon = null;
        base.UnEquip(itemToUnequip, newParnetSlot);
    }

    public override void Equip(UI_Item itemToEquip, UI_ItemSlot parentSlot, bool isEquiped)
    {
        currentWeapon = (PlayerWeaponItem)itemToEquip.itemInfo;
        base.Equip(itemToEquip, parentSlot, isEquiped);
    }
}
