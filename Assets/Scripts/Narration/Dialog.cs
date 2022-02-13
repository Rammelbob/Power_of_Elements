using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Objects/Narration/Dialogue")]
public class Dialog : ScriptableObject
{
    [SerializeField]
    private DialogNode startNode;

    public DialogNode StartNode => startNode;

    public DialogNode currentNode;

    [System.NonSerialized]
    public UnityEvent dialogTriggeredEvent;

    [System.NonSerialized]
    public UnityEvent dialogEndEvent;


    private void OnEnable()
    {
        if (dialogTriggeredEvent == null)
        {
            dialogTriggeredEvent = new UnityEvent();
        }
    }
    
    public void HandleDialog()
    {
        dialogTriggeredEvent.Invoke();
    }

    public void OnEndDialog()
    {
       // dialogTriggeredEvent.Invoke();
    }


}
