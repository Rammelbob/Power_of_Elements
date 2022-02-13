using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField]
    private Dialog dialog;


    private bool speakerNear = false;
    private bool talking = false;
    PlayerInput playerControls;

    private void Awake()
    {
        playerControls = new PlayerInput();
        playerControls.CharacterControls.Talk.started += DialogHandler;
        playerControls.Enable();
    }

    private void DialogHandler(InputAction.CallbackContext obj)
    {

        if (speakerNear)
        {
            talking = true;
            dialog.HandleDialog();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            speakerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            speakerNear = false;
        }
    }

}
