using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : Singleton<InventoryController>
{
    public List<Item> playerItems = new List<Item>();
    

    public void GiveItem(string itemName)
    {
        Item item = ItemDatabase.Instance.GetItem(itemName);
        playerItems.Add(item);
        Debug.Log(playerItems.Count + " items in inventory. Added item: " + itemName);
        foreach (var i in playerItems)
        {
            Debug.Log(i.itemName);
        }
    }

}
