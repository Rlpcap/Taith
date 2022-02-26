using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkToAllVillagersQuest : QuestGiver
{
    public override void Start()
    {

        questName = "Welcome to town";
        questDescription = "Talk to all the villagers";
        goals = new List<QuestGoal>();

        goals.Add(new CollectionGoal(this, "TalkedToMia", "Talk to Mia", false, 0, 1));
        goals.Add(new CollectionGoal(this, "TalkedToRen", "Talk to Ren", false, 0, 1));
        goals.Add(new CollectionGoal(this, "TalkedToElder", "Talk to Elder", false, 0, 1));
        goals.Add(new CollectionGoal(this, "TalkedToIsa", "Talk to Isa", false, 0, 1));


        foreach (CollectionGoal g in goals)
        {
            Debug.Log(g.description);
            g.Init();
        }
        Debug.Log("Quest added!");
        base.Start();

        pv.ToggleQuestsUI();
    }

    public override void CallRewardEvent()
    {

    }
}
