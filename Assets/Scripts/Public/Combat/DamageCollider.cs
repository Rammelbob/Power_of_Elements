using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : CombatCollider
{
    BaseStats myStats;

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
                //stats.TakeDamage(myStats.CurrentWeaponDamage(), this);
            }
        }
    }
}
