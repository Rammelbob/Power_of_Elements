using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCollider : MonoBehaviour
{
    Collider combatCollider;

    public virtual void Awake()
    {
        combatCollider = GetComponent<Collider>();
        combatCollider.gameObject.SetActive(true);
        combatCollider.isTrigger = true;
        combatCollider.enabled = false;
    }

    public void EnableCombatCollider()
    {
        combatCollider.enabled = true;
    }

    public void DisableCombatCollider()
    {
        combatCollider.enabled = false;
    }
}
