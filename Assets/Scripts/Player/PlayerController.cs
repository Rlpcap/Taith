using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : IController
{
    PlayerModel _model;

    public PlayerController(PlayerModel M, PlayerView V)
    {
        _model = M;
    }

    public void OnExecute()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            _model.Move(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));

        if (Input.GetKeyDown(KeyCode.Space))
            _model.Jump();

        if (Input.GetKeyDown(KeyCode.Q))
            _model.StopTime();

        if (Input.GetButtonDown("Fire1"))
            _model.Attack();
    }
}
