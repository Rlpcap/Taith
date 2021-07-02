using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelPortal : MonoBehaviour
{
    public string quest;
    public string task;
    private void OnTriggerEnter(Collider coll)
    {
        var pl = coll.gameObject.GetComponent<PlayerModel>();
        if (pl)
        {
            GameManager.Instance.lastLevelAchieved++;
            QuestManager.Instance.CheckTask(quest, task, true);
        }
    }
}
