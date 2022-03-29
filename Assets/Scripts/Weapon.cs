using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    [Header("Element")]
    public ElementsEnum element;

    public ElementsEnum weakAgainst;
    public ElementsEnum StrongAgainst;

    public Material material;

    [Header("WeaponItems")]
    public GameObject rightHandWeapon;
    public GameObject leftHandWeapon;

    [Header("Light Attack")]
    public Attack first_Light_Attack;

    [Header("Heavy Attack")]
    public Attack first_Heavy_Attack;

    [Header("Running Attack")]
    public Attack running_Attack;

    //[Header("Special Action")]
}
