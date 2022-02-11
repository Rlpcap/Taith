using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTimeFloor : BossFloor
{
    protected override void Start()
    {
        base.Start();
        onPlayerHit += PlayerEffect;
    }

    void PlayerEffect(PlayerModel pl)
    {
        pl.CallStopInTime(1);
    }
}
