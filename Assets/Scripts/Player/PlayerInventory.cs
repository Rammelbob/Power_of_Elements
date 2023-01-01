using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour, ISaveable
{
    public SkinnedMeshRenderer skin;
    public WeaponSlotManager weaponSlotManager;
    PlayerManager playerManager;

    public PlayerWeaponItem currentWeapon;
    public PlayerWeaponItem deafaultWeapon;

    public List<BaseItem> itemsToAdd;

    bool canCollect;
    GameObject collecteable;
    


    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        LoadWeapon(deafaultWeapon);
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
        if (currentWeapon != null)
           playerManager.ui_Inventory_Handler.SubtractStatLevelChange(currentWeapon);
        
        
        currentWeapon = weapon;
        skin.material = weapon.material;
        LoadNewElementWeapon();
        playerManager.ui_Inventory_Handler.AddStatLevelChange(currentWeapon);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            collecteable = other.gameObject;
            canCollect = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            collecteable = null;
            canCollect = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            if (!canCollect && collecteable == null)
            {
                collecteable = other.gameObject;
                canCollect = true;
            }
           
        }
    }

    public void Collect()
    {
        if (canCollect && collecteable != null)
        {
            itemsToAdd.Add(collecteable.GetComponent<Collectable>().GetCollectableItem());
            Destroy(collecteable);
            collecteable = null;
            canCollect = false;
        }
    }

    public object CaptureState()
    {
        return new SaveData
        {
            currentItems = playerManager.ui_Inventory_Handler.GetCurrentItems()
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        itemsToAdd = saveData.currentItems;
    }

    [Serializable]
    private struct SaveData
    {
        public List<BaseItem> currentItems;
    }
}