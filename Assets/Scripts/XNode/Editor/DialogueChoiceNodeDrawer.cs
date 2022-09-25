using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;
using System.Linq;

[CustomNodeEditor(typeof(ChoiceNode))]
public class DialogueChoiceNodeDrawer : NodeEditor
{
    
    private ChoiceNode choiceNode;
    public override void OnBodyGUI()
    {

        if (choiceNode == null)
        {
            choiceNode = target as ChoiceNode;
        }

        serializedObject.Update();

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("entry"));


        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Options"));

    }

    void AddDynamicPorts(Node node)
    {
        node.AddDynamicOutput(typeof(int), fieldName: "myDynamicOutput");
    }
}
