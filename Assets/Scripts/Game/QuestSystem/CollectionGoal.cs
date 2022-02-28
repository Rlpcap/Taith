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
        UIEventHandler.OnItemRemovedToInventory += ItemRemoved;
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
        if (item.itemName == this.itemID)
        {
            Debug.Log("Correct ID!");
            if (item.itemType == Item.ItemTypes.NormalItem)
                this.currentAmmount = item.ammount;
            else
                this.currentAmmount++;

            Debug.Log("CurrentAmmount: " + this.currentAmmount);
            Evaluate();
        }
    }

    void ItemRemoved(Item item, int ammount)
    {
        if (item.itemName == this.itemID)
        {
            this.currentAmmount -= ammount;
        }
    }
}
