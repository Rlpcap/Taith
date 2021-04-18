using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour
{
    public TotemPlatform linkedPlatform;

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.GetComponent<PlayerModel>())
            linkedPlatform.Inactive = false;
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.GetComponent<PlayerModel>())
            linkedPlatform.Inactive = true;
    }
}
