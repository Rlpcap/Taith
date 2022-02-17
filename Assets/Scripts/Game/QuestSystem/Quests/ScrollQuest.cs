using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollQuest : QuestGiver
{
    public PlayerView pv;

    void Awake()
    {
        pv = FindObjectOfType<PlayerView>();

    }
    void Start()
    {

        questName = "Isa's scroll";
        questDescription = "Bring back Isa's scroll";
        goals = new List<QuestGoal>();

        goals.Add(new CollectionGoal(this, "Scroll", "Find the Scroll", false, 0, 1));


        foreach (CollectionGoal g in goals)
        {
            Debug.Log(g.description);
            g.Init();
        }
        Debug.Log("Quest added!");
    }

    public override void CallRewardEvent()
    {
        pv.ShowBookUI();
    }
}
