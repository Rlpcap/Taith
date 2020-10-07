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

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerModel>())
            useWind = true;

    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerModel>())
            useWind = false;
    }

    public void OnFixedUpdate()
    {
        if(useWind)
            _target.GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Acceleration);

    }
}
