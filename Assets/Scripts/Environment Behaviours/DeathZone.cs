using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider coll)
    {
        var pl = coll.GetComponent<PlayerRestart>();
        if (pl)
            pl.CallRestartLevel();
    }
}
