using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Item
{
    public enum ItemTypes { QuestItem, NormalItem };
    public string itemName;
    public bool itemModifier;

    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public ItemTypes itemType;


    [Newtonsoft.Json.JsonConstructor]
    public Item(string _itemName, bool _itemModifier, ItemTypes _itemType)
    {
        this.itemName = _itemName;
        this.itemModifier = _itemModifier;
        this.itemType = _itemType;
    }
}
