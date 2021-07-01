using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCurrent : MonoBehaviour
{
    public float force;

    private void OnTriggerStay(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();
        if(pl)
            pl.GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();
        if (pl)
            pl.OnWaterEnter();
    }

    private void OnTriggerExit(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();
        if (pl)
            pl.OnWaterExit();
    }
}
