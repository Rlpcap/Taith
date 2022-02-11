using System.Collections;
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
        //add method to event listener onenemydeath
    }

    void ItemPickedUp(Item item)
    {
        if (item.itemName == this.itemID)
        {
            this.currentAmmount++;
            Evaluate();
        }
    }
}
