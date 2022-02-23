using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : Singleton<InventoryController>
{
    public List<Item> playerItems = new List<Item>();


    public void GiveItem(string itemName)
    {
        Item item = ItemDatabase.Instance.GetItem(itemName);
        CheckItemType(item);

        foreach (var i in playerItems)
        {
            Debug.Log(i.itemName);
        }
        UIEventHandler.ItemAddedToInventory(item);
    }

    void CheckItemType(Item item)
    {
        if (item.itemType == Item.ItemTypes.QuestItem)
        {
            playerItems.Add(item);
            Debug.Log(playerItems.Count + " items in inventory. Added item: " + item.itemName);
            return;
        }

        if (item.itemType == Item.ItemTypes.NormalItem)
        {
            foreach (var i in playerItems)
            {
                if (item.itemName == i.itemName)
                {
                    item.ammount++;
                    Debug.Log(playerItems.Count + " items in inventory. Added item: " + item.itemName + ". " + "Number of " + item.itemName + " in total: " + item.ammount);
                    return;
                }
            }
            playerItems.Add(item);
            item.ammount++;
            Debug.Log(playerItems.Count + " items in inventory. Added item: " + item.itemName + ". " + "Number of " + item.itemName + " in total: " + item.ammount);
        }

    }

}
