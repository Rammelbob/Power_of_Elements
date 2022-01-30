using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvents : MonoBehaviour
{
    new Collider collider;
    bool isPlayer;
    

    private void Awake()
    {
        isPlayer = gameObject.CompareTag("Player");
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Enemy") && isPlayer) || (other.CompareTag("Player") && !isPlayer))
        {
            List<GameObject> enemies = new List<GameObject>();
            enemies.Add(other.gameObject);
            gameObject.GetComponentInParent<ICombat>().DoAttack(enemies);
            collider.enabled = false;
        }
    }
}
