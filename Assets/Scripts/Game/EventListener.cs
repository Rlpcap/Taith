using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventListener : MonoBehaviour
{
    public delegate void EventHandler();

    public static event EventHandler OnEvent;


    void Start()
    {
        Debug.Log("UIEVENT Loaded");
    }

    public static void EventAdded()
    {

        if (OnEvent != null)
        {
            Debug.Log("OnItemAddedToInventory called!");
            OnEvent();

        }
    }
}
