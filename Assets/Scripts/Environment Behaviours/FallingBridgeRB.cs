using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBridgeRB : MonoBehaviour, IFreezable
{
    public float pushPower;
    Vector3 _velocity;
    Rigidbody _RB;
    bool _hasToFall = false;

    private void Start()
    {
        _RB = GetComponent<Rigidbody>();
    }

    public void Push()
    {
        _RB.isKinematic = false;
        _RB.AddForce(transform.forward * pushPower * Time.deltaTime, ForceMode.Impulse);
        _hasToFall = true;
    }

    public void Freeze()
    {
        _RB.velocity = Vector3.zero;
        _RB.isKinematic = true;
    }

    public void Unfreeze()
    {
        _RB.isKinematic = false;
        _RB.velocity = _velocity;
        if (_hasToFall && _velocity != Vector3.zero) Push();
    }

    public IEnumerator FreezeTime(float freezeTime)
    {
        _velocity = _RB.velocity;
        Freeze();
        yield return new WaitForSeconds(freezeTime);
        Unfreeze();
    }
}
