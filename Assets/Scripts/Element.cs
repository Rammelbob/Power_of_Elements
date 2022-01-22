using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Element")]
public class Element : ScriptableObject
{
    [Header("Element Information")]
    public ElementsEnum element;

    [Header("Weapon Information")]
    public Weapon rightHandWeapon;
    public Weapon leftHandWeapon;

    [Header("Elemental Material")]
    public Material material;
}
