using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyTimeEnemy : LobbyEnemy
{
    private void Start()
    {
        _myPowerAction = _playerModel.StopTime;
    }

    protected override void GetPower()
    {
        //_playerModel.GetPower(_playerModel.StopTime, (int)myPower);
    }
}
