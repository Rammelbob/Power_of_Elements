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

    [Header("Inventory")]
    public Transform slotParent;
    public GameObject inventoryItemPrefab;
    public ScrollRect itemScrollRect;

    [Header("Stats")]
    public Transform statParent;
    public GameObject statPrefab;
    public ScrollRect statScrollRect;

    [Header("Open/Close Inventory")]
    public List<GameObject> showInventory;
    public bool inventoryOpen = false;

    [Header("Texts")]
    public TextMeshProUGUI currencyText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI descriptionNameText;

    public Transform dragableParent;
    public List<UI_EquipmentSlot> equipmentSlots = new List<UI_EquipmentSlot>();
    public UI_ItemSlot consumableSlot;
    public List<UI_WeaponSlot> weaponSlots = new List<UI_WeaponSlot>();
    
    List<UI_Stat> ui_Stats = new List<UI_Stat>();
    UI_Item ui_Item;
    UI_Stat ui_Stat;
    GameObject tempGameObject;
    UI_ItemSlot ui_ItemSlotTemp;
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

    
    public  void ToggleInventory(bool open)
    {
        inventoryOpen = open;
        foreach (GameObject gameObject in showInventory)
        {
            gameObject.SetActive(open);
        }

        foreach (var equipmentSlot in equipmentSlots)
        {
            if (open)
            {
                equipmentSlot.AddStatLevelChanges += AddStatLevelChange;
                equipmentSlot.SubtractStatLevelChanges += SubtractStatLevelChange;
            }
            else
            {
                equipmentSlot.AddStatLevelChanges -= AddStatLevelChange;
                equipmentSlot.SubtractStatLevelChanges -= SubtractStatLevelChange;
            }
        }

        foreach (var weaponSlot in weaponSlots)
        {
            if (open)
                weaponSlot.UnequipWeapon += UnEquipWeapon;
            else
                weaponSlot.UnequipWeapon -= UnEquipWeapon;
            
        }
    }

    private void UnEquipWeapon(PlayerWeaponItem weapon)
    {
        playerManager.playerInventory.LoadWeapon(playerManager.playerInventory.deafaultWeapon);
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

            ui_Stat.OnUIStatSelect += SetStatScrollViewToSelectedChild;
        }
    }

    public void ChangeUILevel(StatEnum itemType, int level)
    {
        ui_Stats.Where(i => i.statType == itemType).First().SetLvl(level);
    }

    public void SetStatScrollViewToSelectedChild(RectTransform selectedChild)
    {
        RectTransform contentPanel = statParent.GetComponent<RectTransform>();

        Vector2 viewportLocalPosition = statScrollRect.viewport.localPosition;
        Vector2 childLocalPosition = selectedChild.localPosition;
        Vector2 result = new Vector2(
            0 - (viewportLocalPosition.x + childLocalPosition.x),
            0 - (viewportLocalPosition.y + childLocalPosition.y));

        contentPanel.localPosition = result;
    }

    public void SetItemScrollViewToSelectedChild(RectTransform selectedChild)
    {
        RectTransform contentPanel = itemParent.GetComponent<RectTransform>();

        Vector2 viewportLocalPosition = itemScrollRect.viewport.localPosition;
        Vector2 childLocalPosition = selectedChild.localPosition;
        Vector2 result = new Vector2(contentPanel.localPosition.x,
            0 - (viewportLocalPosition.y + childLocalPosition.y));

        contentPanel.localPosition = result;
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
                        ui_Item.ChangeCounter(1);
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
            ui_Item.OnUIItemSelect += SetItemScrollViewToSelectedChild;
        }
    }

    private void AddCurrency(int amount)
    {
        currency += amount;
        currencyText.text = currency.ToString();
    }

    #endregion

    #region ItemEvents

    private void OnItemBeginDrag(UI_Item item)
    {
        item.gameObject.transform.SetParent(dragableParent);
    }

    private void OnRightClick(UI_Item item)
    {
        if (item.isEquiped)
            item.parentSlot.UnEquip(item, this);
        else
            switch(item.itemInfo.GetItemType())
            {
                case ItemTypeEnum.Consumable:
                    consumableSlot.SetItemAsChild(item);
                    break;
                case ItemTypeEnum.Weapon:
                    ui_ItemSlotTemp = weaponSlots.Where(i => i.itemParent.childCount == 0).FirstOrDefault();
                    if (ui_ItemSlotTemp)
                        ui_ItemSlotTemp.SetItemAsChild(item);
                    else
                        weaponSlots[0].SetItemAsChild(item);
                    break;
                default:
                    ui_ItemSlotTemp = equipmentSlots.Where(i => i.GetSlotType() == item.itemInfo.GetItemType()).FirstOrDefault();
                    if (ui_ItemSlotTemp)
                        ui_ItemSlotTemp.SetItemAsChild(item);
                    break;
            }
            
    }

    private void OnLeftClick(UI_Item obj)
    {
        descriptionText.text = obj.itemInfo.description;
        descriptionNameText.text = obj.itemInfo.itemName;
    }

    #endregion

    #region ChangeStatLevel
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
    #endregion

    public PlayerWeaponItem GetPlayerWeaponItemIndex(int index)
    {
        if (weaponSlots[index].itemParent.childCount != 0)
        {
            return weaponSlots[index].currentWeapon;
        }
        return null;
    }

    public override void SetItemAsChild(UI_Item newItem)
    {
        Equip(newItem, this, false);
    }

}
