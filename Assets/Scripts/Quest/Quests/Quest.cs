using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[CreateAssetMenu(menuName = "Quest")]
[Serializable]
public class Quest : ScriptableObject
{
    public  List<Goal> goals = new List<Goal>();
    public string questName;
    public string description;
    public bool completed;

    public event Action<Quest> QuestCompleted;

    public void CheckGoals()
    {
        completed = goals.All(g => g.completed);
        if (completed) QuestCompleted?.Invoke(this);
    }

    public void Init()
    {
        foreach (var goal in goals)
        {
            goal.Init();
            goal.GoalCompleted += CheckGoals;
        }
    }
}
