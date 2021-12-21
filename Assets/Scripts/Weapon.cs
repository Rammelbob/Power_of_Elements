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
    public string light_Attack_1;
    public string light_Attack_2;
    public string light_Attack_3;
    public string light_Attack_4;

    [Header("Heavy Attack")]
    public string heavy_Attack_1;
    public string heavy_Attack_2;
}
