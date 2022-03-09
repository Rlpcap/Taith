using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class PickupObject : MonoBehaviour
{
    public string id;

    public string gameObjectName;

    public string sceneID;

    void Awake()
    {
        sceneID = SceneManager.GetActiveScene().name;
        gameObjectName = this.gameObject.name;
    }

    private void Start()
    {
        //StartCoroutine(WaitUp());
    }

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

    public void WaitUp()
    {
        sceneID = SceneManager.GetActiveScene().name;
        gameObjectName = this.gameObject.name;
    }
}
