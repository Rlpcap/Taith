using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyWindEnemy : LobbyEnemy
{
    private void Start()
    {
        _myPowerAction = _playerModel.SuperJump;
    }

    protected override void GetPower()
    {
        //_playerModel.GetPower(_playerModel.SuperJump, (int)myPower);
    }
}
