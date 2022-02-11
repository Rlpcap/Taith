using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireFloor : BossFloor
{
    protected override void Start()
    {
        base.Start();
        onPlayerHit += PlayerEffect;
    }

    void PlayerEffect(PlayerModel pl)
    {
        if (!pl.OnFire)
            pl.SetOnFire(1);
    }
}
