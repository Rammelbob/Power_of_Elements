using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;

public class PrefabOrganisor : EditorWindow
{
    [MenuItem("Window/UI Toolkit/Drag And Drop")]
    public static void ShowExample()
    {
        PrefabOrganisor wnd = GetWindow<PrefabOrganisor>();
        wnd.titleContent = new GUIContent("Drag And Drop");
    }

    public void CreateGUI()
    {
      
    }
}