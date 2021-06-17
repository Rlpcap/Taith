using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBox : PickupObject
{
    // esta clase la voy a cambiar y la voy a usar para que llame a un evento cuando atravieso el trigger

    public override void OnTriggerEnter(Collider coll)
    {
        base.OnTriggerEnter(coll);

        gameObject.SetActive(false);
    }
}
