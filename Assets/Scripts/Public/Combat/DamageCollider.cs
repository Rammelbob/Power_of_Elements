using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageCollider : CombatCollider
{
    BaseStats myStats;

    public override void Awake()
    {
        base.Awake();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            myStats = GetComponentInParent<BaseStats>();
            BaseStats stats = other.GetComponent<BaseStats>();
            if (stats != null)
            {
                myStats.DoDamage(stats);
            }
        }
    }
}
