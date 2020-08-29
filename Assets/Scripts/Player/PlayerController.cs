using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : IController
{
    PlayerModel _model;

    public PlayerController(PlayerView V, PlayerModel M)
    {
        _model = M;
    }

    public void OnExecute()
    {

    }
}
