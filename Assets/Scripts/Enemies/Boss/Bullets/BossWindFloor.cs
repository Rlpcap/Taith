using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWindFloor : BossFloor
{
    BossWindShaderController _BWSC;

    protected override void Start()
    {
        base.Start();
        _BWSC = GetComponent<BossWindShaderController>();
        _BWSC.CallStepWind();
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
            if (!pl.Shielded && !pl.Hovering)
            {
                onPlayerHit(pl);
                StopAllCoroutines();
                GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(Die(1.25f));
            }
        }
    }

    protected override IEnumerator Die(float t)
    {
        yield return UpdateManager.WaitForSecondsCustom(t);
        yield return _BWSC.FadeOutStepWind();
        Destroy(gameObject);
    }
}
