using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeWidth(300)]
public class DialogueNode : BaseNode {

	[Input] public int entry;
	[Output] public int exit;

	public string speakerName;
    public string dialogueLine;


    public override NodeType? GetNodeType()
    {
        return NodeType.TextNode;
    }

    public override string GetSpeaker()
	{
		return speakerName;
    }


	public override string GetText()
	{
		return dialogueLine;
	}
}