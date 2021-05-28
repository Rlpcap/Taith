using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyIceEnemy : LobbyEnemy
{
    private void Start()
    {
        _myPowerAction = _playerModel.IceSpell;
    }

    protected override void GetPower()
    {
        //_playerModel.GetPower(_playerModel.IceSpell, (int)myPower);
    }
}
