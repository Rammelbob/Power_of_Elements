using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public SkinnedMeshRenderer skin;
    WeaponSlotManager weaponSlotManager;
    PlayerStats playerStats;

    ElementsEnum[] playerElements = new ElementsEnum[2] {ElementsEnum.Earth,ElementsEnum.Fire};
    public ElementsEnum currentElement;

    public Weapon[] playerActiveElements;

    private void Awake()
    {
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        playerStats = GetComponent<PlayerStats>();
        SetCurrentElement(0);
    }

    public void SetCurrentElement(int input)
    {
        currentElement = playerElements[input];
        playerStats.weapon = playerActiveElements[input];
        skin.material = playerStats.weapon.material;
        LoadNewElementWeapon();
        Debug.Log($"{currentElement}  {input}");
    }

    public void LoadNewElementWeapon()
    {
        if (playerStats.weapon.rightHandWeapon)
            weaponSlotManager.LoadNewWeaponOnSlot(false, playerStats.weapon);
        if (playerStats.weapon.leftHandWeapon)
            weaponSlotManager.LoadNewWeaponOnSlot(true, playerStats.weapon);
    }

    public void ShowCurrentWeapon(bool showWeapon)
    {
        if (playerStats.weapon.rightHandWeapon)
            weaponSlotManager.ShowCurrentWeapon(false, showWeapon);
        if (playerStats.weapon.leftHandWeapon)
            weaponSlotManager.ShowCurrentWeapon(true, showWeapon);
    }
}
