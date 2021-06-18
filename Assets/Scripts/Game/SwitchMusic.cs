using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMusic : MonoBehaviour
{
    public int musicIndex;

    private void OnTriggerEnter(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();

        if (pl)
            MusicManager.Instance.SwitchMusic(musicIndex);
    }
}
