using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpEnemy : Enemy
{
    public override void Action()
    {
        UpdateManager.Instance.RemoveElementUpdate(this);
        Destroy(gameObject);
        _playerModel.CanTp = true;
    }
}
