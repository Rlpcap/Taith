using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossFloor : MonoBehaviour
{
    public float lifeTime;
    protected Action<PlayerModel> onPlayerHit = delegate { };

    protected virtual void Start()
    {
        StartCoroutine(Die(lifeTime));
    }

    protected virtual IEnumerator Die(float t)
    {
        yield return UpdateManager.WaitForSecondsCustom(t);
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();
        if (pl)
        {
            if (!pl.Shielded)
            {
                onPlayerHit(pl);
                StopAllCoroutines();
                Destroy(gameObject);
            }
        }
    }
}
