using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
public class ItemDatabase : Singleton<ItemDatabase>
{
    private List<Item> _items = new List<Item>();


    protected override void Awake()
    {
        base.Awake();
        BuildDatabase();
    }

    private void BuildDatabase()
    {
        _items = JsonConvert.DeserializeObject<List<Item>>(Resources.Load<TextAsset>("JSON/Items").ToString());
        //Debug.Log(_items[0].itemName);
    }

    public Item GetItem(string _itemName)
    {
        foreach (Item item in _items)
        {
            if(item.itemName == _itemName)
            return item;
        }
        Debug.Log("Couldn't find item: "+ _itemName);
        return null;
    }

}
