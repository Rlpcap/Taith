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

        goals.Add(new CollectionGoal(this, "VillagersTalked", "Talk with all the Villagers", false, 0, 4, TypeOfGoal.Item));


        foreach (QuestGoal g in goals)
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
