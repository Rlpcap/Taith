using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public string itemName;

    public bool itemModifier;

    public Item(string _itemName, bool _itemModifier)
    {
        this.itemName = _itemName;
        this.itemModifier = _itemModifier;
    }
}
