using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour, IUpdate, IFreezable
{
    public int ID;
    protected bool timeStopped = false;

    float gravity = -0.8f;
    //Vector3 _velocity;

    protected bool _falling = false;
    public bool Falling
    {
        get { return _falling; }
    }
    protected bool _hasToFall = false;
    //protected Rigidbody _RB;

    public virtual void Start()
    {
        //_RB = GetComponent<Rigidbody>();
        UpdateManager.Instance.AddElementUpdate(this);
    }

    public virtual void OnUpdate()
    {
        if (_falling) ApplyGravity();
    }

    public virtual void ApplyGravity()
    {
        //_velocity.y += gravity * Time.deltaTime;
        //_RB.AddForce(_velocity * gravityForce * Time.deltaTime);
        transform.position += new Vector3(0, gravity * Time.deltaTime, 0);
    }

    public virtual void Freeze()
    {
    }
    public virtual void Unfreeze()
    {
    }

    public virtual IEnumerator FreezeTime(float f)
    {
        return null;
    }

}
