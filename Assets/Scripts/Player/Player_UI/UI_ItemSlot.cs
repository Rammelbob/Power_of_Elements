using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;


public class UI_ItemSlot : MonoBehaviour, IDropHandler
{
    public ItemTypeEnum slotType;
    public Transform itemParent;
    

    public ItemTypeEnum GetSlotType()
    {
        return slotType;
    }

    public void OnDrop(PointerEventData eventData)
    {
        UI_Item ui_Item = eventData.pointerDrag.GetComponent<UI_Item>();
        if (ui_Item)
        {
            if (ui_Item.itemInfo.values.isEquipped)
                ui_Item.parentSlot.UnEquip(ui_Item, this);
            else
                SetItemAsChild(ui_Item);
        }
    }

    public virtual void SetItemAsChild(UI_Item newItem)
    {
        if (GetSlotType() == newItem.itemInfo.GetItemType())
        {
            if (itemParent.childCount != 0)
            {
                UI_Item currentItem = itemParent.GetChild(0).GetComponent<UI_Item>();
                UnEquip(currentItem, newItem.parentSlot);
            }
            Equip(newItem, this, true);
        }
        else
            newItem.parentSlot.SetItemAsChild(newItem);
    }

    public virtual void UnEquip(UI_Item itemToUnequip, UI_ItemSlot newParnetSlot)
    {
        newParnetSlot.SetItemAsChild(itemToUnequip);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public virtual void Equip(UI_Item itemToEquip, UI_ItemSlot parentSlot, bool isEquipped)
    {
        itemToEquip.transform.SetParent(itemParent);
        itemToEquip.parentSlot = parentSlot;
        itemToEquip.itemInfo.values.isEquipped = isEquipped;
    }
}
