using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPortal : MonoBehaviour
{
    public int myLobbySpawnIndex;

    private void OnTriggerEnter(Collider coll)
    {
        var pl = coll.gameObject.GetComponent<PlayerModel>();

        if (pl)
            GameManager.Instance.lobbySpawnIndex = myLobbySpawnIndex;
    }
}
