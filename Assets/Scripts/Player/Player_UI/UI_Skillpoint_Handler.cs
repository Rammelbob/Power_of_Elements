using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Skillpoint_Handler : UI_ItemSlot
{

    [Header("Inventory")]
    public GameObject inventoryItemPrefab;
    public ScrollRect itemScrollRect;

    [Header("Open/Close Inventory")]
    public List<GameObject> showInventory;
    public bool inventoryOpen = false;

    //[Header("Texts")]
    //public TextMeshProUGUI currencyText;
    //public TextMeshProUGUI descriptionText;
    //public TextMeshProUGUI descriptionNameText;

    public Transform dragableParent;
    //public List<UI_EquipmentSlot> equipmentSlots = new List<UI_EquipmentSlot>();
    public UI_WeaponSlot weaponSlot;

    UI_Item ui_Item;
    GameObject tempGameObject;
    UI_ItemSlot ui_ItemSlotTemp;
    PlayerManager playerManager;

    private int skillPoints = 5;

    public int SkillPoints
    {
        get { return skillPoints; }
        set { if (value >= 0) { skillPoints = value; } }
    }

    private void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
    }

    public void ToggleInventory(bool open)
    {

        inventoryOpen = open;
        foreach (GameObject gameObject in showInventory)
        {
            gameObject.SetActive(open);
        }

        if (open)
            weaponSlot.UnequipWeapon += UnEquipWeapon;
        else
            weaponSlot.UnequipWeapon -= UnEquipWeapon;

    }

    private void UnEquipWeapon(PlayerWeaponItem weapon)
    {
        playerManager.playerInventory.LoadWeapon(playerManager.playerInventory.deafaultWeapon);
    }

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
                //AddCurrency(tradableItem.currencyValue);
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

    private void OnItemBeginDrag(UI_Item item)
    {
        item.gameObject.transform.SetParent(dragableParent);
    }

    private void OnRightClick(UI_Item item)
    {
        if (item.itemInfo.values.isEquipped)
            item.parentSlot.UnEquip(item, this);
        else
            switch (item.itemInfo.GetItemType())
            {
                case ItemTypeEnum.Consumable:
                    //consumableSlot.SetItemAsChild(item);
                    break;
                case ItemTypeEnum.Weapon:
                    //ui_ItemSlotTemp = weaponSlots.Where(i => i.itemParent.childCount == 0).FirstOrDefault();
                    //if (ui_ItemSlotTemp)
                    //    weaponSlot.SetItemAsChild(item);
                    //else
                    weaponSlot.SetItemAsChild(item);
                    break;
                default:
                    //ui_ItemSlotTemp = equipmentSlots.Where(i => i.GetSlotType() == item.itemInfo.GetItemType()).FirstOrDefault();
                    if (ui_ItemSlotTemp)
                        ui_ItemSlotTemp.SetItemAsChild(item);
                    break;
            }
    }

    //public void EnhamceWeapon(TMP_InputField pointsUsed)
    //{
    //    int damagePerSkillPoint = 10;
    //    int usedPoints = Convert.ToInt16(pointsUsed.text);

    //    weaponSlot.currentWeapon.first_Light_Attack.attackDamage += damagePerSkillPoint * usedPoints;

    //    SkillPoints -= usedPoints;

    //    pointsUsed.text = "0";

    //    Debug.Log(weaponSlot.currentWeapon.first_Light_Attack.attackDamage);
    //}

    private void OnLeftClick(UI_Item obj)
    {
        //descriptionText.text = obj.itemInfo.description;
        //descriptionNameText.text = obj.itemInfo.itemName;
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

    public override void SetItemAsChild(UI_Item newItem)
    {
        Equip(newItem, this, false);
    }

}
