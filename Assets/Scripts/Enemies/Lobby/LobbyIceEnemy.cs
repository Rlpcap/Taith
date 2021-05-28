using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyIceEnemy : LobbyEnemy
{
    protected override void Start()
    {
        base.Start();
        _myPowerAction = _playerModel.IceSpell;
    }
}
