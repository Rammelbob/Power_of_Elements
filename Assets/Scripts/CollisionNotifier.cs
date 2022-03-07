using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionNotifier : MonoBehaviour
{
    public UnityEvent<Collider> colliderEntered;
    public UnityEvent<Collider> colliderExit;

    private void Start()
    {
        if (colliderEntered == null && colliderExit == null)
        {
            colliderEntered = new UnityEvent<Collider>();
            colliderExit = new UnityEvent<Collider>();
        }
    }

    private void OnTriggerEnter(Collider other)
    { 
        Debug.Log("Entered");
        colliderEntered.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        colliderExit.Invoke(other);
    }

}
