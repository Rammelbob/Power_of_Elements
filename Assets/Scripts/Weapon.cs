using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    [Header("Weapon Information")]
    public string weaponName;
    public GameObject modelPrefab;

    [Header("Light Attack")]
    public Attack first_Light_Attack;

    [Header("Heavy Attack")]
    public Attack first_Heavy_Attack;
}
