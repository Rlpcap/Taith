using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EndLevelPortal : MonoBehaviour
{
    public int ID;
    public string quest;
    public string task;

    public string portalCompleted;
    private void OnTriggerEnter(Collider coll)
    {
        var pl = coll.gameObject.GetComponent<PlayerModel>();
        if (pl)
        {
            GetComponents<Collider>().ToList().Where(x => x.isTrigger).First().enabled = false;
            GameManager.Instance.CompleteTutorialPortal(ID);
            //QuestManager.Instance.CheckTask(quest, task, true);
            InventoryController.Instance.GiveItem(portalCompleted);
        }
    }
}
