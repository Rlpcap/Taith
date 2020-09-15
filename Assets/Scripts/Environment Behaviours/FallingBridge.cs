using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBridge : FallingObject
{
    public float rotateSpeed;
    public float maxRotateAngle;
    float _currentRotated = 0;
    Vector3 _velocity;
    Rigidbody _childrenRB;

    public override void Start()
    {
        base.Start();
        _childrenRB = GetComponentInChildren<Rigidbody>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (_falling)
            Rotate();
    }

    public void Rotate()
    {
        if (_currentRotated <= maxRotateAngle)
        {
            _velocity.z += rotateSpeed * Time.deltaTime;
            //transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));
            transform.Rotate(_velocity * Time.deltaTime);
            _currentRotated += _velocity.z * Time.deltaTime;
        }
    }

    public void Push()
    {
        //_RB.isKinematic = false;
        if(!timeStopped)
            _falling = true;
        _hasToFall = true;
        _childrenRB.constraints = RigidbodyConstraints.None;
        _childrenRB.constraints = RigidbodyConstraints.FreezeRotationX;
        _childrenRB.constraints = RigidbodyConstraints.FreezeRotationY;
        //_RB.AddForce(transform.forward * pushPower * Time.deltaTime, ForceMode.Impulse);
        //transform.position += transform.forward * rotateSpeed * Time.deltaTime;
    }

    public override void Freeze()
    {
        timeStopped = true;
        _childrenRB.velocity = Vector3.zero;
        _childrenRB.constraints = RigidbodyConstraints.FreezeAll;
        _childrenRB.isKinematic = true;
        //_RB.velocity = Vector3.zero;
        //_RB.constraints = RigidbodyConstraints.FreezeAll;
        //_RB.isKinematic = true;
        _falling = false;
    }

    public override void Unfreeze()
    {
        timeStopped = false;
        //_RB.constraints = RigidbodyConstraints.None;
        //_RB.isKinematic = false;
        if (_hasToFall)
        {
            _falling = true;

        }
    }

    public override IEnumerator FreezeTime(float freezeTime)
    {
        Freeze();
        yield return new WaitForSeconds(freezeTime);
        Unfreeze();
    }
}
