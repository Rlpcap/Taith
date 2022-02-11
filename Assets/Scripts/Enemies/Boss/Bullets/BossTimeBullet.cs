using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTimeBullet : BossBullet
{
    private void OnTriggerEnter(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();
        if (pl)
        {
            if (!pl.Shielded)
            {
                pl.CallStopInTime(1);
            }
            StopAllCoroutines();
            UpdateManager.Instance.RemoveElementUpdate(this);
            Destroy(gameObject);
        }
        else if(coll.gameObject.layer == 9)
        {
            var f = Instantiate(spawnFloor, transform.position, transform.rotation);
            StopAllCoroutines();
            UpdateManager.Instance.RemoveElementUpdate(this);
            Destroy(gameObject);
        }
    }
}
