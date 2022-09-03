using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestQuest : Quest
{
    void Start()
    {
        questName = "test";
        description = "testing quest System";

        foreach (var goal in goals)
        {
            goal.Init();
            goal.GoalCompleted += CheckGoals;
        }
    }
}
