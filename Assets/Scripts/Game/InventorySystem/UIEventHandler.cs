using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEventHandler : MonoBehaviour
{
    public delegate void ItemEventHandler(Item item);

    public static event ItemEventHandler OnItemAddedToInventory;

    void Start()
    {
        Debug.Log("UIEVENT Loaded");
    }

    public static void ItemAddedToInventory(Item item)
    {

        if (OnItemAddedToInventory != null)
        {
            Debug.Log("OnItemAddedToInventory called!");
            OnItemAddedToInventory(item);

        }
    }
}
