using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEnemy : Enemy
{
    public override void Action()
    {
        UpdateManager.Instance.RemoveElementUpdate(this);
        Destroy(gameObject);
        _playerModel.CanFreezeTime = true;
    }
}
