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
    private void Start()
    {
        currentNode = dialog.StartNode;
    }

    //Enable OnGUI after implementing actuall GUI
    //public void OnGUI()
    public void Update()
    {



        if (Input.GetKeyDown(KeyCode.F))
        {
            DisplayDialog();
        }
    }

    private void DisplayDialog()
    {
        if (currentNode == null) { return; }

        nameText.text = currentNode.speakerName;

        if (linePos >= currentNode.textLines.Count)
        {
            linePos = 0;
            LoadNewDialogNode();
            return;
        }
        dialogText.text = currentNode.textLines[linePos++];
    }

    private void LoadNewDialogNode()
    {
        currentNode = currentNode.nextNode;
        DisplayDialog();
    }
}
