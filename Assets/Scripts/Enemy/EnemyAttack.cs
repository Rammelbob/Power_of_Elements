using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Enemy Attack")]
public class EnemyAttack : ScriptableObject
{
    public string attackName;
    public float attackDamage;
    public float staminaCost;
}
