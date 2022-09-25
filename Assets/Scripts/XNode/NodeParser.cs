using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NodeParser : MonoBehaviour
{

    private DialogueGraph graph;

    public TextMeshProUGUI speaker;
    public TextMeshProUGUI dialogue;
    private NPC_DialogueHandler NpcDialogueHandler;
    public bool canTalk = false;
    private PlayerInput playerInput;

    public DialogueOptionsParser optionsParser;

    private Canvas canvas;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Enable();
    }

    void Start()
    {
        canvas = GameObject.Find("DialogueCanvas").GetComponent<Canvas>();
    }
    
    public void SetDialogueGraph(DialogueGraph graph, NPC_DialogueHandler speaker)
    {
        this.graph = graph;
        NpcDialogueHandler = speaker;
        SetStartNode();
    }

    private void OnEnable()
    {
        playerInput.CharacterControls.Talk.started += ctx => DisplayText();
    }


    private void OnDisable()
    {
        playerInput.CharacterControls.Talk.started -= ctx => DisplayText();
    }



    private void SetStartNode()
    {
        foreach (BaseNode node in graph.nodes)
        {
            if (node.GetNodeType() == NodeType.Start)
            {
                graph.current = node;
                break;
            }
        }
    }

    public void DisplayText()
    {
        if (!canTalk) { return; }

        canvas.enabled = true;

        BaseNode baseNode;

        if (graph.current.GetNodeType() == NodeType.Start)
        {
            baseNode = NextNode("exit");
        }
        else
        {
            baseNode = graph.current;
        }

        var type = baseNode.GetNodeType();

        switch (type)
        {
            case NodeType.TextNode:
                speaker.text = baseNode.GetSpeaker();
                dialogue.text = baseNode.GetText();
                NextNode("exit");
                break;


            case NodeType.ChoiceNode:
                speaker.text = baseNode.GetSpeaker();
                dialogue.text = baseNode.GetText();
                DisplayOptions();
                break;

            case NodeType.EventNode:
                var eventNode = baseNode as EventNode;
                eventNode.function.Invoke();
                baseNode = NextNode("exit");
                DisplayText();
                break;

            case NodeType.End:
                canvas.enabled = false;

                var endNode = (EndNode)baseNode;
                if (endNode.nextDialogueGraph != null)
                {
                    NpcDialogueHandler.graph = endNode.nextDialogueGraph;
                }
                else
                {
                    SetStartNode();
                }
                break;

            default:
                break;
        }


    }

    private void DisplayOptions()
    {
        var choiceNoce = graph.current as ChoiceNode;
        var options = choiceNoce.GetOptions();
        optionsParser.ShowOptionButtons(options, 80f, this);
    }


    public void NextNodeFromOption(int index)
    {
        NextNode("Options " + index);
        DisplayText();
    }

    public BaseNode NextNode(string fieldName)
    {
        foreach (var p in graph.current.Ports)
        {
            if (p.fieldName == fieldName)
            {
                graph.current = p.Connection.node as BaseNode;
                break;
            }
        }
        return graph.current;
    }
}
