using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEnemy : Enemy
{
    public float range;
    public float turnSpeed;
    public float timeToSpawn;
    public WindBullet wind;
    bool _isAttacking;

    public override void Start()
    {
        base.Start();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //AimAtTarget();
        TurnWind();

        if (_falling)
            _isAttacking = false;
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
            wind.gameObject.SetActive(true);
        }
        else
        {
            wind.gameObject.SetActive(false);
            wind.useWind = false;
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
        wind.useWind = false;
        _playerModel.GetPower(_playerModel.SuperJump);
        //_playerModel.ActivePower = _playerModel.SuperJump;
        UpdateManager.Instance.RemoveElementUpdate(this);
        Destroy(gameObject);
    }

    public override void GetHitEffect()
    {

    }
}
