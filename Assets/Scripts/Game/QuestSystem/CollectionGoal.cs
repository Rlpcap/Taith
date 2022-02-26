using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionGoal : QuestGoal
{
    public string itemID;

    public CollectionGoal(QuestGiver quest, string itemId, string description, bool completed, int currentAmmount, int requiredAmmount, TypeOfGoal typeOfGoal)
    {
        this.quest = quest;
        this.itemID = itemId;
        this.description = description;
        this.completed = completed;
        this.currentAmmount = currentAmmount;
        this.requiredAmmount = requiredAmmount;
        this.typeOfGoal = typeOfGoal;
    }

    public override void Init()
    {
        base.Init();
        UIEventHandler.OnItemAddedToInventory += ItemPickedUp;
        Debug.Log("COllection goal added!");
        //add method to event listener onenemydeath
        CheckCurrentItems();
    }

    void CheckCurrentItems()
    {
        foreach (Item item in InventoryController.Instance.playerItems)
        {
            if (item.itemName == this.itemID)
            {
                this.currentAmmount++;
                Evaluate();
            }
        }
    }

    void ItemPickedUp(Item item)
    {
        Debug.Log("Checking item ID...");
        if (item.itemName == this.itemID)
        {
            Debug.Log("Correct ID!");
            this.currentAmmount++;
            Debug.Log("CurrentAmmount: " + this.currentAmmount);
            Evaluate();
        }
    }
}
