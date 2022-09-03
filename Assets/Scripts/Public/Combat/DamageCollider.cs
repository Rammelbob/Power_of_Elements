using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageCollider : CombatCollider
{
    BaseStats myStats;

    public static event Action<int> kill;

    public override void Awake()
    {
        base.Awake();
        myStats = GetComponentInParent<BaseStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            //BaseStats stats = GetComponentInParent<BaseStats>();
            BaseStats stats = other.GetComponent<BaseStats>();
            if (stats != null)
            {
                //stats.DamageCalculation(other.gameObject, this);
                var temp = stats.TakeDamage(10, this);
                if (temp != null)
                {
                    kill?.Invoke(temp.id);
                }
            }
        }
    }
}
