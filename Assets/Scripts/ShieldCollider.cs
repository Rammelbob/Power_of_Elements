using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollider : MonoBehaviour
{
    Collider shieldCollider;

    private void Awake()
    {
        shieldCollider = GetComponent<Collider>();
        shieldCollider.gameObject.SetActive(true);
        shieldCollider.isTrigger = true;
        shieldCollider.enabled = false;
    }

    public void EnableShieldCollider()
    {
        shieldCollider.enabled = true;
    }

    public void DisableShieldCollider()
    {
        shieldCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            BaseStats stats = GetComponentInParent<BaseStats>();
            if (stats != null)
            {
                DamageCollider hittingDamageCollider = other.gameObject.GetComponent<DamageCollider>();
                if (hittingDamageCollider != null)
                {
                    if (!stats.blockedColliders.Contains(hittingDamageCollider))
                    {
                        stats.blockedColliders.Add(hittingDamageCollider);
                    }
                }
            }
        }
    }
}
