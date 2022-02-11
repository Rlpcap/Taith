using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWindFloor : BossFloor
{
    protected override void Start()
    {
        base.Start();
        onPlayerHit += PlayerEffect;
    }

    void PlayerEffect(PlayerModel pl)
    {
        pl.SendHovering(1, transform.position);
    }

    protected override void OnTriggerEnter(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();
        if (pl)
        {
            if (!pl.Shielded)
            {
                onPlayerHit(pl);
                StopAllCoroutines();
                GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(Die(1.25f));
            }
        }
    }
}
