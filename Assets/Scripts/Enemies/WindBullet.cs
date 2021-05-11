using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBullet : MonoBehaviour, IFixedUpdate
{
    public float force;
    public float airGravity;
    public ParticleSystem ps;
    PlayerModel _target;
    public bool useWind;

    private void Start()
    {
        UpdateManager.Instance.AddElementFixedUpdate(this);
        ps = GetComponentInChildren<ParticleSystem>();
    }

    void OnTriggerEnter(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();
        if (pl)
        {
            useWind = true;
            pl.OnWind = true;
            pl.CurrentGravityForce = airGravity;
        }
    }

    void OnTriggerExit(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();
        if (pl)
        {
            useWind = false;
            pl.OnWind = false;
            pl.CurrentGravityForce = pl.gravityForce;
        }
    }

    private void OnDisable()
    {
        useWind = false;
        _target.OnWind = false;
        _target.CurrentGravityForce = _target.gravityForce;
    }

    public void OnFixedUpdate()
    {
        if(useWind)
            _target.GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Acceleration);
    }

    public void DestroyComponent()
    {
        UpdateManager.Instance.RemoveElementFixedUpdate(this);
        Destroy(gameObject);
    }

    public WindBullet SetTarget(PlayerModel pl)
    {
        _target = pl;
        return this;
    }
}
