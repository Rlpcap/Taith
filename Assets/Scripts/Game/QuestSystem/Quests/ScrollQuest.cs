using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollQuest : QuestGiver
{
    public override void Start()
    {

        questName = "Isa's scroll";
        questDescription = "Bring back Isa's scroll";
        goals = new List<QuestGoal>();

        goals.Add(new CollectionGoal(this, "Scroll", "Find the Scroll", false, 0, 1, TypeOfGoal.Item));
        goals.Add(new CollectionGoal(this, "TalkedToIsa", "Talk to Isa", false, 0, 2, TypeOfGoal.Interaction));


        foreach (QuestGoal g in goals)
        {
            Debug.Log(g.description);
            g.Init();
        }
        Debug.Log("Quest added!");
        base.Start();
    }

    public override void CallRewardEvent()
    {
        InventoryController.Instance.RemoveItem("Scroll", 1);
        pv.UpdateInventoryUI();
        pv.ShowBookUI();

        FindObjectOfType<Npc_Mia>().StaffQuest();
    }
}
