using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;
using static XNode.Node;

[CustomNodeEditor(typeof(EndNode))]
public class EndNodeDrawer : NodeEditor
{

    private EndNode startNode;

    private static readonly Color backgroundColor = new Color(0.0f, 0.0f, 1f, 1f);
    public override void OnBodyGUI()
    {
        
        if (startNode == null)
        {
            startNode = target as EndNode;
        }
        serializedObject.Update();

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("entry"));

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("nextDialogueGraph"));

    }
}
