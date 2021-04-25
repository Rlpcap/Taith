using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyFireEnemy : LobbyEnemy
{
    protected override void GetPower()
    {
        _playerModel.GetPower(_playerModel.Dash, (int)myPower);
    }
}
