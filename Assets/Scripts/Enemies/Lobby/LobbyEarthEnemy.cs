using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyEarthEnemy : LobbyEnemy
{
    private void Start()
    {
        _myPowerAction = _playerModel.EarthShield;
    }
    protected override void GetPower()
    {
        //_playerModel.GetPower(_playerModel.EarthShield, (int)myPower);
    }
}
