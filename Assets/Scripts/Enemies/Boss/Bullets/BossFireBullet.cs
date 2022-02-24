using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireBullet : BossBullet
{
    private void OnTriggerEnter(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();
        if (pl)
        {
            if (!pl.Shielded && !pl.OnFire)
            {
                pl.SetOnFire(1);
            }
            StopAllCoroutines();

            DestroyMe();
        }
        else if (coll.gameObject.layer == 9)
        {
            if (FloorRay() != default)
            {
                var f = Instantiate(spawnFloor, FloorRay() + new Vector3(0, .1f, 0), transform.rotation);
            }
            StopAllCoroutines();

            DestroyMe();
        }
    }
}
