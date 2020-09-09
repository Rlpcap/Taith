using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour, IUpdate, IFreezable
{
    public int ID;
    public bool timeStopped = false;

    public float gravityForce = 1000;
    float _gravity = -9.81f;
    Vector3 _velocity;

    protected bool _falling = false;
    protected Rigidbody _RB;

    public virtual void Start()
    {
        _RB = GetComponent<Rigidbody>();
        UpdateManager.Instance.AddElementUpdate(this);
    }

    public virtual void OnUpdate()
    {
        if (_falling) ApplyGravity();
    }

    public virtual void ApplyGravity()
    {
        _velocity.y += _gravity * Time.deltaTime;
        _RB.AddForce(_velocity * gravityForce * Time.deltaTime);
    }

    public virtual void Freeze()
    {
    }
    public virtual void Unfreeze(bool wf)
    {
    }

    public virtual IEnumerator FreezeTime(float f)
    {
        return null;
    }

}
