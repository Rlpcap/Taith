using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireBullet : BossBullet
{
    protected override IEnumerator Prepare(float t)
    {
        yield return base.Prepare(t);
        SoundManager.PlaySound(SoundManager.Sound.BossFireBall, transform.position);
    }

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
