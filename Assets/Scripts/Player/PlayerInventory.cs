using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
    public SkinnedMeshRenderer skin;
    WeaponSlotManager weaponSlotManager;
    PlayerManager playerManager;

    public PlayerWeaponItem[] playerActiveWeapons;
    public PlayerWeaponItem weapon;

    public List<BaseItem> itemsToAdd;


    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        SetCurrentElement(0);
        foreach (var item in itemsToAdd)
        {
            playerManager.ui_Inventory_Handler.AddItem(item);
        }
    }

    public void SetCurrentElement(int input)
    {
        if (input < playerActiveWeapons.Length)
        {
            weapon = playerActiveWeapons[input];
            skin.material = weapon.material;
            LoadNewElementWeapon();
        }
    }

    public void LoadNewElementWeapon()
    {
        if (weapon.rightHandWeapon)
            weaponSlotManager.LoadNewWeaponOnSlot(false, weapon);
        if (weapon.leftHandWeapon)
            weaponSlotManager.LoadNewWeaponOnSlot(true, weapon);
    }

    public void ShowCurrentWeapon(bool showWeapon)
    {
        if (weapon.rightHandWeapon)
            weaponSlotManager.ShowCurrentWeapon(false, showWeapon);
        if (weapon.leftHandWeapon)
            weaponSlotManager.ShowCurrentWeapon(true, showWeapon);
    }
}