using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollQuest : QuestGiver
{
    void Start()
    {
        questName = "Mia's scroll";
        questDescription = "Bring back Mia's scroll";

        goals = new List<QuestGoal>
        {
            new CollectionGoal(this, "Scroll", "Find the Scroll", false, 0, 1)
        };
        Debug.Log("Quest added!");
    }
}
