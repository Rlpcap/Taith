using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimneySmoke : MonoBehaviour
{
    public float setOnFireDuration;
    private void OnTriggerEnter(Collider coll)
    {
        var pl = coll.gameObject.GetComponent<PlayerModel>();

        if (pl)
        {
            if (!pl.Shielded)
                pl.SetOnFire(setOnFireDuration);
        }
    }
}