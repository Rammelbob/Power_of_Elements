using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Player Attack")]
public class PlayerAttack : ScriptableObject
{
    public string attackName;
    public float attackDamage;
    public float staminaCost;

    public PlayerAttack nextLightAttack;
    public PlayerAttack nextHeavyAttack;
}
