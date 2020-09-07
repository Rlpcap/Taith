using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBridge : FallingObject
{
    public float pushPower;

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public void Push()
    {
        _RB.isKinematic = false;
        //_falling = true;
        _RB.AddForce(transform.forward * pushPower * Time.deltaTime, ForceMode.Impulse);
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
        _RB.isKinematic = false;
        //if (wasFalling) _falling = true;
    }

    public override IEnumerator FreezeTime(float freezeTime)
    {
        bool wasFalling = _falling;
        Freeze();
        yield return new WaitForSeconds(freezeTime);
        Unfreeze(wasFalling);
    }
}
