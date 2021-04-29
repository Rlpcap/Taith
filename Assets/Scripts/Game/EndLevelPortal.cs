using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider coll)
    {
        var pl = coll.gameObject.GetComponent<PlayerModel>();
        if (pl)
            GameManager.Instance.lastLevelAchieved++;
    }
}
