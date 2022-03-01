using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : Singleton<InventoryController>
{
    public List<Item> playerItems = new List<Item>();

    public List<ItemOnSceneData> itemsOnScenes = new List<ItemOnSceneData>();

    public void AddItemOnItemsScenes(ItemOnSceneData name)
    {
        itemsOnScenes.Add(name);
    }

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

    public void RemoveItem(string itemName, int ammount)
    {
        Item item = ItemDatabase.Instance.GetItem(itemName);
        CheckItemTypeRemove(item, ammount);
        UIEventHandler.ItemRemovedToInventory(item, ammount);
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
            if (playerItems.Contains(item))
                foreach (var i in playerItems)
                {
                    if (item.itemName == i.itemName)
                    {
                        item.ammount++;
                        Debug.Log(playerItems.Count + " items in inventory. Added item: " + item.itemName + ". " + "Number of " + item.itemName + " in total: " + item.ammount);
                        return;
                    }
                }
            else
            {
                playerItems.Add(item);
                item.ammount++;

            }
            Debug.Log(playerItems.Count + " items in inventory. Added item: " + item.itemName + ". " + "Number of " + item.itemName + " in total: " + item.ammount);
        }

    }

    void CheckItemTypeRemove(Item item, int ammount)
    {

        if (item.itemType == Item.ItemTypes.NormalItem)
        {
            for (int i = playerItems.Count - 1; i >= 0; i--)
            {
                if (item.itemName == playerItems[i].itemName)
                {
                    item.ammount -= ammount;
                    if (item.ammount <= 0)
                        playerItems.Remove(item);

                    Debug.Log(playerItems.Count + " items in inventory. Added item: " + item.itemName + ". " + "Number of " + item.itemName + " in total: " + item.ammount);
                    return;
                }
            }
        }

    }

}
