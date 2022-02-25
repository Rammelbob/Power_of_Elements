using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSequencer : MonoBehaviour
{
    [SerializeField]
    Dialog dialog;

    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI dialogText;

    private DialogNode currentNode;

    int linePos = 0;

    private void Awake()
    {
        currentNode = dialog.StartNode;
    }


    private void OnEnable()
    {
        dialog.dialogTriggeredEvent.AddListener(DisplayDialog);
    }

    private void OnDisable()
    {
        dialog.dialogTriggeredEvent.RemoveListener(DisplayDialog);
    }

    private void DisplayDialog()
    {
        if (currentNode == null)
        {
            ClearDialog();
            return;
        }
        nameText.text = currentNode.speakerName;

        if (linePos >= currentNode.textLines.Count)
        {
            linePos = 0;
            currentNode = currentNode.nextNode;
            return;
        }
        dialogText.text = currentNode.textLines[linePos++];
    }

    private void ClearDialog()
    {
        Debug.Log("Remove DialogBox");
        currentNode = dialog.StartNode;
    }
}
