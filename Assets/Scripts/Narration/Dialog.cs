using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Narration/Dialogue")]
public class Dialog : ScriptableObject
{
    [SerializeField]
    private DialogNode startNode;



    public DialogNode StartNode => startNode;

}
