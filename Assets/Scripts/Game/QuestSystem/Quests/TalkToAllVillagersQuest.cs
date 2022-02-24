﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkToAllVillagersQuest : QuestGiver
{
    public PlayerView pv;

    void Awake()
    {
        pv = FindObjectOfType<PlayerView>();

    }
    public override void Start()
    {

        questName = "Welcome to town";
        questDescription = "Talk to all the villagers";
        goals = new List<QuestGoal>();

        goals.Add(new CollectionGoal(this, "VillagerTalked", "Talk to all the villagers", false, 0, 4));


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

    }
}