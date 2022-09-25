using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    PlayerManager playerManager;
    public List<Quest> currentQuests;
    public Quest testQuest;

    void Start()
    {
        playerManager = GetComponent<PlayerManager>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            AddQuest(testQuest);
        }
    }

    public void AddQuest(Quest newQuest)
    {
        if (!currentQuests.Contains(newQuest))
        {
            newQuest.Init();
            newQuest.QuestCompleted += CompleteQuest;
            currentQuests.Add(newQuest);
        }
    }

    public void CompleteQuest(Quest completedQuest)
    {
        if (currentQuests.Contains(completedQuest))
        {
            Debug.Log("give reward");
            currentQuests.Remove(completedQuest);
        }
        
    }
}
