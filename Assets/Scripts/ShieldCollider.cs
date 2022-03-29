using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollider : CombatCollider
{
    public List<DamageCollider> blockedColliders = new List<DamageCollider>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {

            DamageCollider hittingDamageCollider = other.gameObject.GetComponent<DamageCollider>();
            if (hittingDamageCollider != null)
            {
                if (!blockedColliders.Contains(hittingDamageCollider))
                {
                    blockedColliders.Add(hittingDamageCollider);
                }
            }
        }
    }
}
