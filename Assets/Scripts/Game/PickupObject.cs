using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupObject : MonoBehaviour
{
    public string id;

    public virtual void OnTriggerEnter(Collider coll)
    {
        var pl = coll.gameObject.GetComponent<PlayerModel>();

        if (pl)
        {
/*
            //GameManager.Instance.inventoryList.Add(id);
            if (task != "")
                QuestManager.Instance.CheckTask(quest, task, true);
*/

        }

    }
}
