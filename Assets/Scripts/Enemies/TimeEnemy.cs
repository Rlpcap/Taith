using System.Collections;
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
        StopAllCoroutines();
        //_playerModel.CanFreezeTime = true;

        _playerModel.GetPower(_playerModel.StopTime, (int)myPower);
        //_playerModel.ActivePower = _playerModel.StopTime;
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        _anim.SetTrigger("die");
        yield return new WaitForSeconds(2.08f);
        UpdateManager.Instance.RemoveElementUpdate(this);
        Destroy(gameObject);
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
