﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiasHatQuest : QuestGiver
{
    public PlayerView pv;

    void Awake()
    {
        pv = FindObjectOfType<PlayerView>();

    }
    public override void Start()
    {

        questName = "Mia's hat";
        questDescription = "Bring back Mia's hat";
        goals = new List<QuestGoal>();

        goals.Add(new CollectionGoal(this, "MiasHat", "Find the hat", false, 0, 1));


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