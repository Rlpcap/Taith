using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBox : MonoBehaviour
{
    // esta clase la voy a cambiar y la voy a usar para que llame a un evento cuando atravieso el trigger

    private void OnTriggerEnter(Collider coll)
    {
        var pl = coll.gameObject.GetComponent<PlayerModel>();
        
        if(pl)
        {
            QuestManager.Instance.CheckTask("The Hat Quest","Get the hat",true);
            Destroy(this.gameObject);
        }

    }
}
