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

    public Element[] playerActiveElements;

    private void Awake()
    {
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        playerStats = GetComponent<PlayerStats>();
        SetCurrentElement(0);
    }

    public void SetCurrentElement(int input)
    {

        currentElement = playerElements[input];
        playerStats.element = playerActiveElements[input];
        skin.material = playerStats.element.material;
        LoadCurrentElementWeapon();
        Debug.Log($"{currentElement}  {input}");
    }

    public void LoadCurrentElementWeapon()
    {
        if (playerStats.element.rightHandWeapon)
            weaponSlotManager.LoadWeaponOnSlot(false, playerStats.element.rightHandWeapon);
        if (playerStats.element.leftHandWeapon)
            weaponSlotManager.LoadWeaponOnSlot(true, playerStats.element.leftHandWeapon);
    }

    public void UnLoadWeapons()
    {
        if (playerStats.element.rightHandWeapon)
            weaponSlotManager.LoadWeaponOnSlot(false);
        if (playerStats.element.leftHandWeapon)
            weaponSlotManager.LoadWeaponOnSlot(true);
    }
}
