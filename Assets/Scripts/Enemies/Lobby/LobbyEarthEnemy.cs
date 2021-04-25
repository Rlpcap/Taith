using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyEarthEnemy : LobbyEnemy
{
    protected override void GetPower()
    {
        _playerModel.GetPower(_playerModel.EarthShield, (int)myPower);
    }
}
