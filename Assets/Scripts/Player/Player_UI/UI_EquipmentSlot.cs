using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public ItemTypeEnum slotType;
    public event Action<EquipmentItem> AddStatLevelChanges, SubtractStatLevelChanges;
    EquipmentItem equipmentItem;

    public override ItemTypeEnum GetSlotType()
    {
        return slotType;
    }

    public override void SetItemAsChild(UI_Item newItem)
    {
        if (newItem.itemInfo.GetItemType() == slotType)
        {
            if (itemParent.childCount != 0)
            {
                UI_Item currentItem = itemParent.GetChild(0).GetComponent<UI_Item>();
                UnEquip(currentItem, newItem.parentSlot.SetItemAsChild);
            }
            newItem.transform.SetParent(itemParent);
            newItem.parentSlot = this;
            newItem.isEquiped = true;
            equipmentItem = (EquipmentItem)newItem.itemInfo;
            AddStatLevelChanges?.Invoke(equipmentItem);
        }
        else
            newItem.parentSlot.SetItemAsChild(newItem);
    }

    public override void UnEquip(UI_Item itemToUnequip, Action<UI_Item> setItemAsChild)
    {
        equipmentItem = (EquipmentItem)itemToUnequip.itemInfo;
        SubtractStatLevelChanges?.Invoke(equipmentItem);
        base.UnEquip(itemToUnequip, setItemAsChild);
    }
}
