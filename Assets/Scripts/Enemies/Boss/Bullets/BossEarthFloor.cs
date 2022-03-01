using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEarthFloor : BossFloor
{
    PlayerModel _target;

    protected override void Start()
    {
        base.Start();
        GetComponent<Renderer>().material.SetFloat("_IceMudLerp1", 1);
    }

    protected override void OnTriggerEnter(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();
        if (pl)
            _target = pl;
    }

    protected override IEnumerator Die(float t)
    {
        yield return UpdateManager.WaitForSecondsCustom(t);
        if (_target)
            _target.UnMud();
        Destroy(gameObject);
    }
}
