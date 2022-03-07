using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Elements : MonoBehaviour
{
    GameObject player;
    Canvas canvas;
    CollisionNotifier collisionNotifier;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Main Camera");
        canvas = GetComponent<Canvas>();
        collisionNotifier = gameObject.GetComponentInParent<CollisionNotifier>();

        collisionNotifier.colliderEntered.AddListener(ShowIcon);
        collisionNotifier.colliderExit.AddListener(HideIcon);
    }

    private void ShowIcon(Collider collider)
    {
        if (collider.tag != "Player") { return; }

        Debug.Log("Invoked");
        canvas.enabled = true;
    }

    private void HideIcon(Collider collider)
    {
        if (collider.tag != "Player") { return; }
        canvas.enabled = false;
    }

    private void OnDisable()
    {
        collisionNotifier.colliderEntered.RemoveListener(ShowIcon);
        collisionNotifier.colliderExit.RemoveListener(HideIcon);
    }


    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
    }
}
