using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
    public SkinnedMeshRenderer skin;
    public WeaponSlotManager weaponSlotManager;
    PlayerManager playerManager;

    public PlayerWeaponItem currentWeapon;
    public PlayerWeaponItem deafaultWeapon;

    public List<BaseItem> itemsToAdd;


    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        LoadWeapon(deafaultWeapon);
        foreach (var item in itemsToAdd)
        {
            playerManager.ui_Inventory_Handler.AddItem(item);
           // playerManager.ui_Skillpoint_Handler.AddItem(item);
        }
    }

    public void SetCurrentWeapon(int input)
    {
        if(playerManager.ui_Inventory_Handler == null)
        {
            return;
        }

        if (input < playerManager.ui_Inventory_Handler.weaponSlots.Count)
        {
            PlayerWeaponItem slotWeapon = playerManager.ui_Inventory_Handler.GetPlayerWeaponItemIndex(input);
            if (slotWeapon == null)
                LoadWeapon(deafaultWeapon);
            else if (slotWeapon != currentWeapon)
                LoadWeapon(slotWeapon);
        }
    }

    public void LoadWeapon(PlayerWeaponItem weapon)
    {
        currentWeapon = weapon;
        skin.material = weapon.material;
        LoadNewElementWeapon();
    }

    public void LoadNewElementWeapon()
    {
        if (currentWeapon)
        {
            if (currentWeapon.rightHandWeapon)
                weaponSlotManager.LoadNewWeaponOnSlot(currentWeapon);
        }
    }

    public void ShowCurrentWeapon(bool showWeapon)
    {
        if (currentWeapon)
        {
            if (currentWeapon.rightHandWeapon)
                weaponSlotManager.ShowCurrentWeapon(showWeapon);
        }
    }
}