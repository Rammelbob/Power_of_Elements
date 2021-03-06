using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Player Weapon")]
public class PlayerWeaponItem : BaseItem
{
    [Header("Element")]
    public ElementsEnum element;

    public ElementsEnum weakAgainst;
    public ElementsEnum StrongAgainst;

    public Material material;
    public GameObject attackVisualEffect;

    [Header("WeaponItems")]
    public GameObject rightHandWeapon;
    public GameObject leftHandWeapon;

    [Header("Light Attack")]
    public PlayerAttack first_Light_Attack;

    [Header("Heavy Attack")]
    public PlayerAttack first_Heavy_Attack;

    [Header("Running Attack")]
    public PlayerAttack running_Attack;

    public override ItemTypeEnum GetItemType()
    {
        return ItemTypeEnum.Weapon;
    }

    //[Header("Special Action")]
}
