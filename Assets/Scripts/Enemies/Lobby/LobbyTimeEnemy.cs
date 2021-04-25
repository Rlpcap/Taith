using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyTimeEnemy : LobbyEnemy
{
    protected override void GetPower()
    {
        _playerModel.GetPower(_playerModel.StopTime, (int)myPower);
    }
}
