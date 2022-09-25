using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint(1.0f, 0.4f, 0.4f)]
public class EndNode : BaseNode {

    [Input] public int entry;

    [SerializeField]
    public DialogueGraph nextDialogueGraph = null;

    public override NodeType? GetNodeType()
    {
        return NodeType.End;
    }
}