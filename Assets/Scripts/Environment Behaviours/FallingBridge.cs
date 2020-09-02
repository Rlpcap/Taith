using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBridge : FallingObject
{
    public float pushPower;

    public void Push()
    {
        _RB.isKinematic = false;
        _RB.AddForce(transform.forward * pushPower * Time.deltaTime, ForceMode.Impulse);
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
        _RB.isKinematic = false;
    }

    public override IEnumerator FreezeTime(float freezeTime)
    {
        Freeze();
        yield return new WaitForSeconds(freezeTime);
        Unfreeze();
    }
}
