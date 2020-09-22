using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : IController
{
    PlayerModel _model;
    CameraFollow _camera;

    public PlayerController(PlayerModel M, PlayerView V)
    {
        _model = M;
    }

    public void OnExecute()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        //Vector3 _dir = _camera.faceFoward.transform.position - _camera.transform.position;


        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            _model.Move(new Vector3(moveX, 0, moveZ));

        if (Input.GetKeyDown(KeyCode.Space))
            _model.Jump();

        if (Input.GetKeyDown(KeyCode.Q))
            _model.StopTime();

        if (Input.GetButtonDown("Fire1"))
            _model.Attack();
    }
}
