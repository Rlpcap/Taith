using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollQuest : QuestGiver
{
    void Start()
    {
        questName = "Mia's scroll";
        questDescription = "Bring back Mia's scroll";
        goals = new List<QuestGoal>();

        goals.Add(new CollectionGoal(this, "Scroll", "Find the Scroll", false, 0, 1));


        foreach (CollectionGoal g in goals)
        {
            Debug.Log(g.description);
            g.Init();
        }
        Debug.Log("Quest added!");
    }
}
