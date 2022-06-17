using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public event Action<EquipmentItem> AddStatLevelChanges, SubtractStatLevelChanges;
    EquipmentItem equipmentItem;

    public override void UnEquip(UI_Item itemToUnequip, UI_ItemSlot newParnetSlot)
    {
        equipmentItem = (EquipmentItem)itemToUnequip.itemInfo;
        SubtractStatLevelChanges?.Invoke(equipmentItem);
        base.UnEquip(itemToUnequip, newParnetSlot);
    }

    public override void Equip(UI_Item itemToEquip, UI_ItemSlot parentSlot, bool isEquiped)
    {
        equipmentItem = (EquipmentItem)itemToEquip.itemInfo;
        AddStatLevelChanges?.Invoke(equipmentItem);
        base.Equip(itemToEquip, parentSlot, isEquiped);
    }
}
