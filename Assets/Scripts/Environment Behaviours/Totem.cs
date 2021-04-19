using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour
{
    public List<TotemPlatform> linkedPlatforms = new List<TotemPlatform>();

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.GetComponent<PlayerModel>())
        {
            foreach (var item in linkedPlatforms)
            {
                item.Inactive = false;
            }
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.GetComponent<PlayerModel>())
        {
            foreach (var item in linkedPlatforms)
            {
                item.Inactive = true;
            }
        }
    }
}
