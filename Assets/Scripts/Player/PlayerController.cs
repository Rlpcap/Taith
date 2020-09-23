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
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        //Vector3 _dir = _camera.faceFoward.transform.position - _camera.transform.position;


<<<<<<< HEAD
        //if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        _model.Move(moveX, moveZ, new Vector3(moveX, 0, moveZ));

        if (Input.GetKeyDown(KeyCode.LeftShift))
            _model.Dash();
=======
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            _model.Move(new Vector3(moveX, 0, moveZ));
>>>>>>> fd9bc2aee2a2ec0de6ec28b0c653dfddd315ced6

        if (Input.GetKeyDown(KeyCode.Space))
            _model.Jump();

        if (Input.GetKeyDown(KeyCode.Q))
            _model.StopTime();

        if (Input.GetButtonDown("Fire1"))
            _model.Attack();
    }
}
