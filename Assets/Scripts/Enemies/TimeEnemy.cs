﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEnemy : MovingEnemy
{
    public float actionDuration;

    public override void Action()
    {
        StartCoroutine(SpeedUp());
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        canShoot = true;
    }

    public override void OnDeath()
    {
        UpdateManager.Instance.RemoveElementUpdate(this);
        Destroy(gameObject);
        //_playerModel.CanFreezeTime = true;

        _playerModel.GetPower(_playerModel.StopTime, (int)myPower);
        //_playerModel.ActivePower = _playerModel.StopTime;
    }

    IEnumerator SpeedUp()
    {
        var normalSpeed = speed;
        var normalRotSpeed = rotSpeed;
        speed *= 3;
        rotSpeed *= 3;
        yield return new WaitForSeconds(actionDuration);
        speed = normalSpeed;
        rotSpeed = normalRotSpeed;
    }
}
