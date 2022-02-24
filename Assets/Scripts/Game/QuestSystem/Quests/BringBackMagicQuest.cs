using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringBackMagicQuest : QuestGiver
{
    public override void Start()
    {

        questName = "Bring back the magic";
        questDescription = "Complete all the portals";
        goals = new List<QuestGoal>();

        goals.Add(new CollectionGoal(this, "TimePortal", "Complete the Time Portal", false, 0, 1));
        goals.Add(new CollectionGoal(this, "EarthPortal", "Complete the Earth Portal", false, 0, 1));
        goals.Add(new CollectionGoal(this, "WindPortal", "Complete the Wind Portal", false, 0, 1));
        goals.Add(new CollectionGoal(this, "IcePortal", "Complete the Ice Portal", false, 0, 1));
        goals.Add(new CollectionGoal(this, "FirePortal", "Complete the Fire Portal", false, 0, 1));


        foreach (CollectionGoal g in goals)
        {
            Debug.Log(g.description);
            g.Init();
        }
        Debug.Log("Quest added!");
        base.Start();
    }

    public override void CallRewardEvent()
    {
        EventListener.EventAdded();
    }
}
