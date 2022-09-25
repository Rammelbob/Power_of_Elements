using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class NPC_DialogueHandler : MonoBehaviour
{

    private NodeParser nodeParser;
    public DialogueGraph graph;

    private void Awake()
    {
        nodeParser = GameObject.Find("DialogueCanvas").GetComponent<NodeParser>();
    }

    private void OnTriggerEnter(Collider other)
    {
        nodeParser.SetDialogueGraph(graph, this);
        nodeParser.canTalk = true;
    }

    private void OnTriggerExit(Collider other)
    {
        nodeParser.canTalk = false;
    }
}
