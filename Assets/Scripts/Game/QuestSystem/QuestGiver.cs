﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestGiver : MonoBehaviour
{
    public List<QuestGoal> goals { get; set; }
    public string questName;
    public string questDescription;
    public bool completed;

    public Item itemReward;

    public PlayerView pv;

    public virtual void Awake()
    {
        pv = FindObjectOfType<PlayerView>();
    }

    public virtual void Start()
    {
        UIEventHandler.UpdateQuestsUI();
    }

    public void CheckGoals()
    {
        /* if (goals.All(g => g.completed))
         {
             if (completed) GiveReward();
         }*/

        completed = goals.All(g => g.completed);
    }

    public void GiveReward()
    {
        //givereward;
        if (itemReward != null)
        {
            InventoryController.Instance.GiveItem(itemReward.ToString());
        }
        CallRewardEvent();
    }

    public virtual void CallRewardEvent()
    {

    }

}
