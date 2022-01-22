using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public SkinnedMeshRenderer skin;
    WeaponSlotManager weaponSlotManager;
    PlayerStats playerStats;

    ElementsEnum[] playerElements = new ElementsEnum[4] {ElementsEnum.Earth,ElementsEnum.Fire,ElementsEnum.Air,ElementsEnum.Water};
    public ElementsEnum currentElement;

    public Element[] playerActiveElements;

    private void Awake()
    {
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        playerStats = GetComponent<PlayerStats>();
        SetCurrentElement(0);
    }

    private void Start()
    {
        LoadCurrentElementWeapon();
    }

    public void SetCurrentElement(int input)
    {
        currentElement = playerElements[input];
        playerStats.element = playerActiveElements[input];
        skin.material = playerStats.element.material;
        LoadCurrentElementWeapon();
        Debug.Log($"{currentElement}  {input}");
    }

    private void LoadCurrentElementWeapon()
    {
        if (playerStats.element.rightHandWeapon)
            weaponSlotManager.LoadWeaponOnSlot(playerStats.element.rightHandWeapon, false);
        if (playerStats.element.leftHandWeapon)
            weaponSlotManager.LoadWeaponOnSlot(playerStats.element.leftHandWeapon, true);
    }
}
