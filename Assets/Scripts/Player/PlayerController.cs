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

        _model.onStoppedInTime += _view.CallOnStoppedInTime;
        _model.onFreeze += _view.CallOnFreeze;
        _model.onFire += _view.CallOnFire;
        _model.onWindJump += _view.SpawnWindJump;
        _model.onFireDash += _view.PlayFireDash;
        _model.onShield += _view.SpawnEarthShield;
        _model.onLaser += _view.SpawnLaser;
        _model.onStopTime += _view.SpawnStopTimeBubble;
        _model.onGetPower += _view.NewPower;
        _model.onMove += _view.RunAnim;
        _model.onJump += _view.Jump;
        _model.onMudJump += _view.MudJump;
        _model.onMudMove += _view.MudMove;
        _model.onCast += _view.Cast;
        _model.onCast += _view.UsePower;
        _model.onCheckGround += _view.GroundCheck;
        _model.onAttack += _view.Attack;
        _model.onInteractableEnter += _view.InteractEnter;
        _model.onInteractableExit += _view.InteractExit;
        _model.onInteract += _view.Interact;
        _model.onPortalTrigger += _view.PortalTrigger;
        _model.onPausedGame += _view.OnPause;
        _model.onUnpausedGame += _view.OnUnpause;
        _model.onWaterEnter += _view.OnWaterStart;
        _model.onWaterExit += _view.OnWaterEnd;
        _model.onUpdateInventoryUI += _view.UpdateInventoryUI;
    }

    public void OnExecute()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        _model.Move(moveX, moveZ);

        if (Input.GetKeyDown(KeyCode.Space))
            _model.Jump();

        if (Input.GetButtonDown("Fire1"))
            _model.Attack();

        if (Input.GetButtonDown("Fire2"))
            _model.UsePower();

        if (Input.GetKeyDown(KeyCode.E))
            _model.Interact();

        
    }
}
