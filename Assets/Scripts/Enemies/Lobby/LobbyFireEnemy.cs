using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyFireEnemy : LobbyEnemy
{
    protected override void Start()
    {
        base.Start();
        _myPowerAction = _playerModel.Dash;
    }
}
