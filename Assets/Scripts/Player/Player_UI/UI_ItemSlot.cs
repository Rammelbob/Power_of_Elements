using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;


public class UI_ItemSlot : MonoBehaviour, IDropHandler
{
    public Transform itemParent;

    public virtual ItemTypeEnum GetSlotType()
    {
        return ItemTypeEnum.Consumable;
    }

    public void OnDrop(PointerEventData eventData)
    {
        UI_Item ui_Item = eventData.pointerDrag.GetComponent<UI_Item>();
        if (ui_Item)
        {
            if (ui_Item.isEquiped)
            {
               ui_Item.parentSlot.UnEquip(ui_Item, SetItemAsChild);
            }
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
                newItem.parentSlot.SetItemAsChild(currentItem);
            }
            newItem.transform.SetParent(itemParent);
            newItem.parentSlot = this;
            newItem.isEquiped = true;
        }
        else
            newItem.parentSlot.SetItemAsChild(newItem);
    }

    public virtual void UnEquip(UI_Item itemToUnequip, Action<UI_Item> setItemAsChild)
    {
        setItemAsChild(itemToUnequip);
    }
}
