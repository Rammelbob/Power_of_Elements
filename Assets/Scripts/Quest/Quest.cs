using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Quest : MonoBehaviour
{
    public  List<Goal> goals = new List<Goal>();
    public string questName;
    public string description;
    public bool completed;
    

    public void CheckGoals()
    {
        completed = goals.All(g => g.completed);
        if (completed) GiveReward();
    }

    void GiveReward()
    {
        Debug.Log("give reward");
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.L))
    //    {
    //        kill?.Invoke(0);
    //    }

    //    if (Input.GetKeyDown(KeyCode.O))
    //    {
    //        kill?.Invoke(1);
    //    }
    //}
}
