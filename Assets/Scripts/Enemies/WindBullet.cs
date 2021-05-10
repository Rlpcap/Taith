using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBullet : MonoBehaviour, IFixedUpdate
{
    public float force;
    public ParticleSystem ps;
    PlayerModel _target;
    public bool useWind;

    private void Start()
    {
        UpdateManager.Instance.AddElementFixedUpdate(this);
        ps = GetComponentInChildren<ParticleSystem>();
        _target = FindObjectOfType<PlayerModel>();
    }

    void OnTriggerEnter(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();
        if (pl)
        {
            useWind = true;
            pl.OnWind = true;
        }
    }

    void OnTriggerExit(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();
        if (pl)
        {
            useWind = false;
            pl.OnWind = false;
        }
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
}
