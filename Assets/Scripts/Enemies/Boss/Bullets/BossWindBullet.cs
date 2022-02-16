using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWindBullet : BossBullet
{
    public BossWindEffect windEffectPF;

    private void OnTriggerEnter(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();
        if (pl)
        {
            if (!pl.Shielded && !pl.Hovering)
            {
                var w = Instantiate(windEffectPF, transform.position, new Quaternion(180, transform.rotation.y, transform.rotation.z, transform.rotation.w));
                pl.SendHovering(1, w.transform.position);
            }
            StopAllCoroutines();

            DestroyMe();
        }
        else if (coll.gameObject.layer == 9)
        {
            var f = Instantiate(spawnFloor, transform.position, new Quaternion(180, transform.rotation.y, transform.rotation.z, transform.rotation.w));
            StopAllCoroutines();

            DestroyMe();
        }
    }
}
