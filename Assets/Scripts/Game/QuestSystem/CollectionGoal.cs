﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionGoal : QuestGoal
{
    public string itemID;

    public CollectionGoal(QuestGiver quest, string itemId, string description, bool completed, int currentAmmount, int requiredAmmount)
    {
        this.quest = quest;
        this.itemID = itemId;
        this.description = description;
        this.completed = completed;
        this.currentAmmount = currentAmmount;
        this.requiredAmmount = requiredAmmount;
    }

    public override void Init()
    {
        base.Init();
        UIEventHandler.OnItemAddedToInventory += ItemPickedUp;
        Debug.Log("COllection goal added!");
        //add method to event listener onenemydeath
    }

    void ItemPickedUp(Item item)
    {
        Debug.Log("Checking item ID...");
        if (item.itemName == this.itemID)
        {
            Debug.Log("Correct ID!");
            this.currentAmmount++;
            Debug.Log("CurrentAmmount: "+this.currentAmmount);
            Evaluate();
        }
    }
}
