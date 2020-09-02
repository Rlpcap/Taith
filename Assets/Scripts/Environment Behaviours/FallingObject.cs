using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour, IUpdate, IFreezable
{
    public int ID;
    public bool timeStopped = false;


    protected Rigidbody _RB;

    private void Start()
    {
        _RB = GetComponent<Rigidbody>();
        UpdateManager.Instance.AddElementUpdate(this);
    }

    public virtual void OnUpdate()
    {
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
