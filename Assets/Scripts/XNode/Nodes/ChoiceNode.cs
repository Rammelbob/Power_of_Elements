using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using XNodeEditor;
using UnityEditor;

public class ChoiceNode : BaseNode
{

    [Input] public int entry;
    [Output(dynamicPortList = true)] public string[] Options;

    
    public string[] GetOptions()
    {
        return Options;
    }

    public override NodeType? GetNodeType()
    {
        return NodeType.ChoiceNode;
    }

}