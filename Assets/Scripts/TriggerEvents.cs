using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvents : MonoBehaviour
{
    public CombatManager combatManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            List<GameObject> enemies = new List<GameObject>();
            enemies.Add(other.gameObject);
            combatManager.DoDamage(enemies);
        }
    }
}
