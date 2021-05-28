using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyFireEnemy : LobbyEnemy
{
    private void Start()
    {
        _myPowerAction = _playerModel.Dash;
    }

    protected override void GetPower()
    {
        //_playerModel.GetPower(_playerModel.Dash, (int)myPower);
    }
}
