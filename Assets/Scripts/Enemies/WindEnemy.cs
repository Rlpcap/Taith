using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEnemy : Enemy
{
    public float range;
    public float turnSpeed;
    public float timeToSpawn;
    WindBullet _wind;
    bool _isAttacking;

    public override void Start()
    {
        base.Start();
        _wind = GetComponentInChildren<WindBullet>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //AimAtTarget();
        TurnWind();
    }

    private void AimAtTarget()
    {
        var nextForward = (_playerModel.transform.position - transform.position).normalized;
        nextForward.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, nextForward, turnSpeed);
    }

    private void TurnWind()
    {
        if (_isAttacking)
        {
            _wind.gameObject.SetActive(true);
        }
        else
        {
            _wind.gameObject.SetActive(false);
            _wind.useWind = false;
        }

    }

    public override void Action()
    {
        _isAttacking = CheckIfAttacking(_isAttacking);
    }

    bool CheckIfAttacking(bool a)
    {
        if (a)
            return a = false;
        else
            return a = true;
    }
    public override void OnDeath()
    {
        _wind.useWind = false;
        _playerModel.GetPower(_playerModel.SuperJump);
        //_playerModel.ActivePower = _playerModel.SuperJump;
        UpdateManager.Instance.RemoveElementUpdate(this);
        Destroy(gameObject);
    }
}
