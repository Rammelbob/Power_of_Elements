using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;

public class UI_Inventory_Handler : UI_ItemSlot
{

    [Header("Prefabs / TransformParents")]
    public GameObject inventoryItemPrefab;
    public Transform statParent;
    public GameObject statPrefab;
    public Transform dragableParent;
    public Transform slotParent;

    [Header("Open/Close Inventory")]
    public List<GameObject> showInventory;
    bool inventoryOpen = false;

    [Header("Texts")]
    public TextMeshProUGUI currencyText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI descriptionNameText;

    List<UI_ItemSlot> itemSlots = new List<UI_ItemSlot>();
    
    List<UI_Stat> ui_Stats = new List<UI_Stat>();
    UI_Item ui_Item;
    UI_Stat ui_Stat;
    GameObject tempGameObject;
    PlayerManager playerManager;
    int currency = 0;

    private void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
    }

    private void Start()
    {
        foreach (var stat in playerManager.playerStats.statsArray)
        {
            CreateUIStat(stat.statType, stat.statvalues.statLevel);
        }
    }

    public void ToggleInventory()
    {
        if (inventoryOpen)
            ToggleInventory(false);
        else
            ToggleInventory(true);
    }

    private void ToggleInventory(bool open)
    {
        inventoryOpen = open;
        foreach (GameObject gameObject in showInventory)
        {
            gameObject.SetActive(open);
        }


        UI_ItemSlot itemSlotTemp;
        foreach (Transform itemSlot in slotParent)
        {
            itemSlotTemp = itemSlot.GetComponent<UI_ItemSlot>();
            if (itemSlotTemp)
            {
                itemSlots.Add(itemSlotTemp);
                if (itemSlotTemp.GetSlotType() != ItemTypeEnum.Consumable)
                {
                    UI_EquipmentSlot ui_EquipmentSlot = (UI_EquipmentSlot)itemSlotTemp;
                    if (open)
                    {
                        ui_EquipmentSlot.AddStatLevelChanges += AddStatLevelChange;
                        ui_EquipmentSlot.SubtractStatLevelChanges += SubtractStatLevelChange;
                    }
                    else
                    {
                        ui_EquipmentSlot.AddStatLevelChanges -= AddStatLevelChange;
                        ui_EquipmentSlot.SubtractStatLevelChanges -= SubtractStatLevelChange;
                    }
                   
                }
            }
        }
    }


    #region Change UI
    private void CreateUIStat(StatEnum statType, int level)
    {
        tempGameObject = Instantiate(statPrefab, statParent);
        ui_Stat = tempGameObject.GetComponent<UI_Stat>();
        if (ui_Stat)
        {
            ui_Stat.SetStat(statType, level);
            ui_Stats.Add(ui_Stat);
        }
    }

    public void ChangeUILevel(StatEnum itemType, int level)
    {
        ui_Stats.Where(i => i.statType == itemType).First().SetLvl(level);
    }
    #endregion

    #region AddItem
    public void AddItem(BaseItem itemToAdd)
    {
        switch (itemToAdd.GetItemType())
        {
            case ItemTypeEnum.Consumable:
                foreach (Transform item in itemParent)
                {
                    ui_Item = item.gameObject.GetComponent<UI_Item>();
                    if (ui_Item.itemInfo == itemToAdd)
                    {
                        ui_Item.IncreaseCounter(1);
                        return;
                    }
                }
                CreateNewItem(itemToAdd);
                break;
            case ItemTypeEnum.Currency:
                TradableItem tradableItem = (TradableItem)itemToAdd;
                AddCurrency(tradableItem.currencyValue);
                break;
            default:
                CreateNewItem(itemToAdd);
                break;
        }
    }

    private void CreateNewItem(BaseItem newItem)
    {
        tempGameObject = Instantiate(inventoryItemPrefab, itemParent);
        ui_Item = tempGameObject.GetComponent<UI_Item>();
        if (ui_Item)
        {
            ui_Item.SetItem(newItem);
            ui_Item.parentSlot = this;

            ui_Item.OnItemBeginDrag += OnItemBeginDrag;
            ui_Item.OnItemLeftClick += OnLeftClick;
            ui_Item.OnItemRightClick += OnRightClick;
        }
    }

    private void AddCurrency(int amount)
    {
        currency += amount;
        currencyText.text = currency.ToString();
    }

    #endregion

    #region ItemEvents

    private void OnItemBeginDrag(UI_Item obj)
    {
        obj.gameObject.transform.SetParent(dragableParent);
    }

    private void OnRightClick(UI_Item obj)
    {
        if (obj.isEquiped)
            obj.parentSlot.UnEquip(obj, SetItemAsChild);
        else
            itemSlots.Where(i => i.GetSlotType() == obj.itemInfo.GetItemType()).First().SetItemAsChild(obj);
    }

    private void OnLeftClick(UI_Item obj)
    {
        descriptionText.text = obj.itemInfo.description;
        descriptionNameText.text = obj.itemInfo.itemName;
    }

    #endregion

    public void AddStatLevelChange(EquipmentItem equipmentItem)
    {
        foreach (var statBuff in equipmentItem.statBuffs)
        {
            ChangeUILevel(statBuff.statType,
                playerManager.playerStats.GetStatByEnum(statBuff.statType)
                .statvalues.ChangeStatLevel(statBuff.amount));
        }   
    }

    public void SubtractStatLevelChange(EquipmentItem equipmentItem)
    {
        foreach (var statBuff in equipmentItem.statBuffs)
        {
            ChangeUILevel(statBuff.statType,
                playerManager.playerStats.GetStatByEnum(statBuff.statType)
                .statvalues.ChangeStatLevel(-statBuff.amount));
        }
    }

    public override ItemTypeEnum GetSlotType()
    {
        return ItemTypeEnum.Default;
    }

    public override void SetItemAsChild(UI_Item newItem)
    {
        newItem.transform.SetParent(itemParent);
        newItem.parentSlot = this;
        newItem.isEquiped = false;
    }
}
