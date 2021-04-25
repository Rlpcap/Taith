using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyWindEnemy : LobbyEnemy
{
    protected override void GetPower()
    {
        _playerModel.GetPower(_playerModel.SuperJump, (int)myPower);
    }
}
