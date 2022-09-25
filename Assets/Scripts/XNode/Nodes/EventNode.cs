using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XNode;

[NodeWidth(300)]
public class EventNode : BaseNode {

    [Input] public int entry;
    [Output] public int exit;


    public UnityEvent function;

    public override NodeType? GetNodeType()
    {
        return NodeType.EventNode;
    }

}