using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEarthFloor : BossFloor
{
    PlayerModel _target;

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
            _target.OnMud = false;
        Destroy(gameObject);
    }
}
