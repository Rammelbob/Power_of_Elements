using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Objects/Narration/DialogueNode")]
public class DialogNode : ScriptableObject
{

    public List<string> textLines;
    public string speakerName;
    public DialogNode nextNode;

}
