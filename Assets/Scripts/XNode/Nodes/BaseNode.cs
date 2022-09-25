using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BaseNode : Node {

    public virtual NodeType? GetNodeType()
    {
        return null;
    }


	public virtual string GetSpeaker()
    {
        return "";
    }
    
    public virtual string GetText()
    {
        return "";
    }

    public virtual Sprite GetSprite()
    {
        return null;
    }
}