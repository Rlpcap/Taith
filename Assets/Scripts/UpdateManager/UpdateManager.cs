using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    static UpdateManager _instance;
    public static UpdateManager Instance
    {
        get { return _instance; }
        private set { }
    }

    List<IUpdate> allUpdateElements = new List<IUpdate>();
    List<IFixedUpdate> allFixedUpdateElements = new List<IFixedUpdate>();

    void Awake()
    {
        _instance = this;
    }

    void Update()
    {
        for (int i = 0; i < allUpdateElements.Count; i++)
        {
            allUpdateElements[i].OnUpdate();
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < allFixedUpdateElements.Count; i++)
        {
            allFixedUpdateElements[i].OnFixedUpdate();
        }
    }

    public void AddElementUpdate(IUpdate element)
    {
        if (!allUpdateElements.Contains(element))
            allUpdateElements.Add(element);
    }

    public void RemoveElementUpdate(IUpdate element)
    {
        if (allUpdateElements.Contains(element))
            allUpdateElements.Remove(element);
    }

    public void AddElementFixedUpdate(IFixedUpdate element)
    {
        if (!allFixedUpdateElements.Contains(element))
            allFixedUpdateElements.Add(element);
    }

    public void RemoveElementFixedUpdate(IFixedUpdate element)
    {
        if (allFixedUpdateElements.Contains(element))
            allFixedUpdateElements.Remove(element);
    }
}
