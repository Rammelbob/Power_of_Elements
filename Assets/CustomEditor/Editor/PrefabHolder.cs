using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
public class PrefabHolder : EditorWindow
{

    static List<GameObject> prefabs = new List<GameObject>();
    Vector2 scrollPosition;

    [MenuItem("Tools/Prefab Holder")]
    public static void ShowWindow()
    {
        GetWindow(typeof(PrefabHolder));
        GetPrefabs();
    }

    private static void GetPrefabs()
    {

        string[] search_results = System.IO.Directory.GetFiles("Assets/Prefabs/", "*.prefab", System.IO.SearchOption.AllDirectories);

        foreach (string search_result in search_results)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath(search_result, typeof(GameObject)) as GameObject;
            prefabs.Add(prefab);
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Objects", EditorStyles.boldLabel);

        scrollPosition = GUILayout.BeginScrollView(
             scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height));

        int count = 0;
        GUILayout.BeginArea(new Rect(0, 0, position.width, position.height));
        GUILayout.BeginHorizontal();

        for(int i = 0; i < 50; i++)
        {
            GUILayout.Label(AssetPreview.GetAssetPreview(prefabs.ElementAt(0)));
        }

        //for (int j = 0; j < prefabs.Count; j++)
        //{
        //    count++;
        //    //GUILayout.Label(AssetPreview.GetAssetPreview(prefabs.ElementAt(j)), GUILayout.Width(100), GUILayout.Height(100));
        //    float x = position.width;
        //    float y = count * position.width / 6;
        //    if (y > x)
        //    {
        //        count = 0;
        //        GUILayout.EndHorizontal();
        //        GUILayout.BeginHorizontal(GUILayout.MaxWidth(position.width));
        //    }
        //}
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        GUILayout.EndScrollView();
    }

}
