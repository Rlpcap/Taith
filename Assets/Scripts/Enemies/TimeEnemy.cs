using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEnemy : Enemy
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
        _playerModel.CanFreezeTime = true;
    }

    IEnumerator SpeedUp()
    {
        var normalSpeed = speed;
        speed *= 2;
        yield return new WaitForSeconds(actionDuration);
        speed = normalSpeed;
    }



}
