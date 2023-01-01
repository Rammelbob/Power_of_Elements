using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCollider : MonoBehaviour
{
    Collider combatCollider;
    AudioSource audioSource;

    public virtual void Awake()
    {
        combatCollider = GetComponent<Collider>();
        combatCollider.gameObject.SetActive(true);
        combatCollider.isTrigger = true;
        combatCollider.enabled = false;

        audioSource = GetComponent<AudioSource>();
    }

    public void EnableCombatCollider()
    {
        combatCollider.enabled = true;
        if (audioSource != null)
        {
            audioSource.time = 0.19f;
            audioSource.Play();
        }
    }

    public void DisableCombatCollider()
    {
        combatCollider.enabled = false;
        if (audioSource != null)
        {
            audioSource.Stop();
        }
       
    }
}
