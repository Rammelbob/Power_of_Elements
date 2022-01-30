using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalMovement : MonoBehaviour
{
    Rigidbody rb;
    public float waterUpSpeed;
    public float startUpTime;
    float upTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        upTime = startUpTime;
    }

    private void Update()
    {
        if (upTime <= 0)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            Destroy(gameObject, 2f);
        }
        else
        {
            upTime -= Time.deltaTime;
            rb.velocity = Vector3.up * waterUpSpeed;
        }
    }

}
