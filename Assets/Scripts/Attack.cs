using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Attack")]
public class Attack : ScriptableObject
{
    public string attackName;
    public float attackDamage;
    public float attackForce;

    public Attack nextLightAttack;
    public Attack nextHeavyAttack;
}
