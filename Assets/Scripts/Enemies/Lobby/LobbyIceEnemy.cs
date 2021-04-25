using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyIceEnemy : LobbyEnemy
{
    protected override void GetPower()
    {
        _playerModel.GetPower(_playerModel.IceSpell, (int)myPower);
    }
}
