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

    public override void OnDeath()
    {
        UpdateManager.Instance.RemoveElementUpdate(this);
        Destroy(gameObject);
        //_playerModel.CanFreezeTime = true;

        _playerModel.GetPower(_playerModel.StopTime);
        //_playerModel.ActivePower = _playerModel.StopTime;
    }

    IEnumerator SpeedUp()
    {
        var normalSpeed = speed;
        speed *= 3;
        yield return new WaitForSeconds(actionDuration);
        speed = normalSpeed;
    }
}
