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
        SetChildOf();
    }

    void SetChildOf()
    {
        Ray ray = new Ray(transform.position, -Vector3.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, 1 << 9))
        {
            transform.SetParent(hit.transform);
        }
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
