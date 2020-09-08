using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpEnemy : Enemy
{
    public override void Action()
    {
        var tempIndex = UnityEngine.Random.Range(0, waypoints.Count);
        transform.position = waypoints[tempIndex].position;
        if (tempIndex < waypoints.Count - 1)
            _index = tempIndex++;
        else
            _index = 0;
    }

    public override void OnDeath()
    {
        UpdateManager.Instance.RemoveElementUpdate(this);
        Destroy(gameObject);
        _playerModel.CanTp = true;
    }
}
