using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;
using UnityEngine.UIElements;
using UnityEngine.Animations.Rigging;
using UnityEngine.ProBuilder.Shapes;
using static XNode.Node;
using static UnityEditor.PlayerSettings;
using UnityEditorInternal.VR;

[CustomNodeEditor(typeof(DialogueNode))]
public class DialogueNodeDrawer : NodeEditor
{

    Vector2 ScrollPos;
    private DialogueNode dialogueNode;
    private bool showDialogueSettings;
    private bool showRichText;

    public string txt = "";
    private int currentNodeTab = -1;


    public override void OnBodyGUI()
    {
        if (dialogueNode == null)
        {
            dialogueNode = target as DialogueNode;
        }
        serializedObject.Update();

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("entry"));

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("exit"));


        showDialogueSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showDialogueSettings, "Node Settings");
        if (showDialogueSettings)
        {
            EditorGUILayout.PrefixLabel("Speaker");
            dialogueNode.speakerName = EditorGUILayout.TextField(dialogueNode.speakerName);
            EditorGUILayout.PrefixLabel("Dialogue");

            EditorGUILayout.PrefixLabel("Show RichText");
            showRichText = EditorGUILayout.Toggle(showRichText);
            
            currentNodeTab = GUILayout.Toolbar(currentNodeTab, new string[] { "B", "I", "U", "S", "Colour" }, GUILayout.Width(260));
            ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos, GUILayout.Height(80));

            dialogueNode.dialogueLine = GUILayout.TextArea(dialogueNode.dialogueLine, new GUIStyle(EditorStyles.textArea) { wordWrap = true, richText = showRichText }, GUILayout.ExpandHeight(true), GUILayout.Height(300));
            TextEditor editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);

            var selectedText = editor.SelectedText;
            var pos = editor.selectIndex;

            switch (currentNodeTab)
            {
                case -1:
                    break;
                case 0:
                    WrapSelectedText("b", pos, editor.SelectedText);
                    break;
                case 1:
                    WrapSelectedText("i", pos, editor.SelectedText);
                    break;
                case 2:
                    WrapSelectedText("u", pos, editor.SelectedText);
                    break;
                case 3:
                    WrapSelectedText("s", pos, editor.SelectedText);
                    break;
                default:
                    break;
            }
            currentNodeTab = -1;
            //  ReplaceText(currentNodeTab,, );

            EditorGUILayout.EndScrollView();

        }

        EditorGUILayout.EndFoldoutHeaderGroup();

    }

    public void WrapSelectedText(string type, int pos, string selectedText)
    {

        if (selectedText.Contains($"<{type}>"))
        {
            dialogueNode.dialogueLine = dialogueNode.dialogueLine.Remove(pos, selectedText.Length);
            selectedText = selectedText.Replace($"<{type}>", "").Replace($"</{type}>", "");
            dialogueNode.dialogueLine = dialogueNode.dialogueLine.Insert(pos, selectedText);
        }
        else
        {
            dialogueNode.dialogueLine = dialogueNode.dialogueLine.Remove(pos, selectedText.Length);
            dialogueNode.dialogueLine = dialogueNode.dialogueLine.Insert(pos, $"<{type}>{selectedText}</{type}>");
        }


    }

    //public override void AddContextMenuItems(GenericMenu menu)
    //{
    //    base.AddContextMenuItems(menu);
    //    if (Selection.objects.Length == 1 && target is XNode.Node)
    //    {
    //        menu.AddItem(new GUIContent("MyItem", "Click"), false, () => Click());
    //    }
    //}



    private void Click()
    {
        Debug.Log("Click");
    }

}
