using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollQuest : QuestGiver
{
    void Start()
    {
        questName = "Mia's scroll";
        questDescription = "Bring back Mia's scroll";

        goals.Add(new CollectionGoal(this, "Scroll", "Find the Scroll", false, 0, 1));

    }
}
