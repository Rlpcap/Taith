using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFloor : FallingObject
{
    public FallingBridge bridge;

    public float speed;
    public float amount;

    public float fallingTime;
    bool _almostFalling = false;

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (_almostFalling && !timeStopped)
            Shake();
    }

    private void Shake()
    {
        _RB.transform.position = new Vector3(_RB.transform.position.x + Mathf.Sin(Time.time * speed + .5f) * amount, _RB.transform.position.y + Mathf.Sin(Time.time * speed - .5f) * amount, _RB.transform.position.z + Mathf.Sin(Time.time * speed) * amount);
    }

    public override void Freeze()
    {
        timeStopped = true;
        _RB.velocity = Vector3.zero;
        _RB.constraints = RigidbodyConstraints.FreezeAll;
        _RB.isKinematic = true;
        //_falling = false;
    }

    public override void Unfreeze(bool wasFalling)
    {
        timeStopped = false;
        _RB.constraints = RigidbodyConstraints.None;
        _RB.constraints = RigidbodyConstraints.FreezeRotation;

        if (_falling) _RB.isKinematic = false;
        //if (wasFalling) _falling = true;
    }

    public override IEnumerator FreezeTime(float freezeTime)
    {
        bool wasFalling = _falling;
        Freeze();
        yield return new WaitForSeconds(freezeTime);
        Unfreeze(wasFalling);
    }

    public IEnumerator StartFalling()
    {
        if(bridge) bridge.Push();

        _almostFalling = true;
        yield return new WaitForSeconds(fallingTime);
        _almostFalling = false;
        _falling = true;

        if(!timeStopped) _RB.isKinematic = false;

        //if(!timeStopped) _falling = true;
    }
}
