using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEventHandler : MonoBehaviour
{
    public delegate void ItemEventHandler(Item item);

    public static event ItemEventHandler OnItemAddedToInventory;

    public delegate void InteractionEventHandler(Item item);

    public static event InteractionEventHandler OnInteractionAdded;

    public delegate void QuestsEventHandler();

    public static event QuestsEventHandler OnQuestUpdated;

    void Start()
    {
        Debug.Log("UIEVENT Loaded");
        OnQuestUpdated += FindObjectOfType<PlayerView>().UpdateQuestsUI;
    }

    public static void ItemAddedToInventory(Item item)
    {

        if (OnItemAddedToInventory != null)
        {
            Debug.Log("OnItemAddedToInventory called!");
            OnItemAddedToInventory(item);

        }
    }

    public static void InteractionAddedToInventory(Item item)
    {

        if (OnItemAddedToInventory != null)
        {
            Debug.Log("OnItemAddedToInventory called!");
            OnItemAddedToInventory(item);

        }
    }

    public static void UpdateQuestsUI()
    {
        
        if (OnQuestUpdated != null)
        {
            OnQuestUpdated();
        }
    }
}
