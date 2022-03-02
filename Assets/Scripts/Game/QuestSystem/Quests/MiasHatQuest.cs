using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiasHatQuest : QuestGiver
{

    public override void Start()
    {

        questName = "Mia's staff";
        questDescription = "Bring back Mia's staff";
        goals = new List<QuestGoal>();

        goals.Add(new CollectionGoal(this, "Staff", "Find the staff", false, 0, 1, TypeOfGoal.Item));
        goals.Add(new CollectionGoal(this, "TalkedToMia", "Talk to Mia", false, 0, 2, TypeOfGoal.Interaction));


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
        //EventListener.EventAdded();
        FindObjectOfType<Npc_Mia>().ShowTutorialPortal();
        InventoryController.Instance.RemoveItem("Staff", 1);
        pv.UpdateInventoryUI();

    }
}
