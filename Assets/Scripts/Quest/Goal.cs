using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Goal
{
    public int requiredID;
    public bool isKillGoal;
    public string description;
    public bool completed;
    public int currentAmount;
    public int requiredAmount;
    public event Action GoalCompleted;


    public void Init()
    {
        if (isKillGoal)
        {
            DamageCollider.kill += AddCurrent;
        }
    }

    public void Evaluate()
    {
        if (currentAmount >= requiredAmount)
        {
            Complete();
        }
    }

    void AddCurrent(int id)
    {
        if (requiredID == id)
        {
            currentAmount++;
            Evaluate();
        }
    }

    public void Complete()
    {
        completed = true;
        Debug.Log("goal complete");
        DamageCollider.kill -= AddCurrent;
        GoalCompleted?.Invoke();
       
    }
}
