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
    public bool almostFalling = false;
    bool falling = false;

    public override void OnUpdate()
    {
        if (almostFalling && !timeStopped)
            Shake();
    }

    public override void Freeze()
    {
        timeStopped = true;
        _RB.velocity = Vector3.zero;
        _RB.constraints = RigidbodyConstraints.FreezeAll;
        _RB.isKinematic = true;
    }

    public override void Unfreeze()
    {
        timeStopped = false;
        _RB.constraints = RigidbodyConstraints.None;

        if(falling) _RB.isKinematic = false;
    }

    private void Shake()
    {
        _RB.transform.position = new Vector3(_RB.transform.position.x + Mathf.Sin(Time.time * speed + .5f) * amount, _RB.transform.position.y + Mathf.Sin(Time.time * speed - .5f) * amount, _RB.transform.position.z + Mathf.Sin(Time.time * speed) * amount);
    }

    public IEnumerator StartFalling()
    {
        if(bridge) bridge.Push();

        falling = true;
        almostFalling = true;
        yield return new WaitForSeconds(fallingTime);
        almostFalling = false;

        if(!timeStopped) _RB.isKinematic = false;
    }

    public override IEnumerator FreezeTime(float freezeTime)
    {
        Freeze();
        yield return new WaitForSeconds(freezeTime);
        Unfreeze();
    }
}
