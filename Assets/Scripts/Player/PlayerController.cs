using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : IController
{
    PlayerModel _model;
    PlayerView _view;

    public PlayerController(PlayerModel M, PlayerView V)
    {
        _model = M;
        _view = V;

        _model.onFireDash += _view.PlayFireDash;
        _model.onShield += _view.SpawnEarthShield;
        _model.onLaser += _view.SpawnLaser;
        _model.onStopTime += _view.SpawnStopTimeBubble;
        _model.onGetPower += _view.NewPower;
        _model.onMove += _view.RunAnim;
        _model.onJump += _view.Jump;
        _model.onCast += _view.Cast;
        _model.onCast += _view.UsePower;
        _model.onCheckGround += _view.GroundCheck;
        _model.onAttack += _view.Attack;
    }

    public void OnExecute()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        //Vector3 _dir = _camera.faceFoward.transform.position - _camera.transform.position;

        //if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        _model.Move(moveX, moveZ);

        if (Input.GetKeyDown(KeyCode.LeftShift))
            _model.Dash();

        //if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        //    _model.Move(moveX, moveZ, new Vector3(moveX, 0, moveZ));

        if (Input.GetKeyDown(KeyCode.Space))
            _model.Jump();

        if (Input.GetButtonDown("Fire1"))
            _model.Attack();

        if (Input.GetButtonDown("Fire2"))
            _model.UsePower();
    }
}
