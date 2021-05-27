using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBullet : MonoBehaviour, IUpdate, IFixedUpdate
{
    public float force;
    public float airGravity;
    public ParticleSystem ps;
    PlayerModel _target;
    public bool useWind;

    bool _collidingWithGround = false;
    public bool CollidingWithGround { get { return _collidingWithGround; } set { _collidingWithGround = value; } }

    BoxCollider _collider;
    float _colliderSizeZ;
    float _colliderCenterZ;
    RaycastHit _hitInfo;
    float _zPercent;
    public float ZPercent { get { return _zPercent; } }

    private void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);
        UpdateManager.Instance.AddElementFixedUpdate(this);
        ps = GetComponentInChildren<ParticleSystem>();
        _collider = GetComponent<BoxCollider>();
        _colliderSizeZ = _collider.size.z;
        _colliderCenterZ = _collider.center.z;
        _target = FindObjectOfType<PlayerModel>();//**ESTO HAY QUE SACARLO, ES SOLO PARA MOSTRARLE AL PROFE ALGO**
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

    public void OnUpdate()
    {
        _collidingWithGround = Physics.Raycast(transform.position, transform.forward, out _hitInfo, _colliderSizeZ * transform.localScale.z, 1 << 9);

        if (_collidingWithGround)
        {
            _collider.size = new Vector3(_collider.size.x, _collider.size.y, ((_hitInfo.point - transform.position).magnitude)/transform.localScale.z);
            _collider.center = new Vector3(_collider.center.x, _collider.center.y, ((_hitInfo.point - transform.position).magnitude/2)/transform.localScale.z);
            _zPercent = ((((_hitInfo.point - transform.position).magnitude) / transform.localScale.z) / _colliderSizeZ) * 100;
            //_collider.size = new Vector3(_collider.size.x, _collider.size.y, _colliderSizeZ/2);
            //_collider.center = new Vector3(_collider.center.x, _collider.center.y, _colliderCenterZ/2);
        }
        else
        {
            _collider.size = new Vector3(_collider.size.x, _collider.size.y, _colliderSizeZ);
            _collider.center = new Vector3(_collider.center.x, _collider.center.y, _colliderCenterZ);
        }
    }

    public void OnFixedUpdate()
    {
        if(useWind)
            _target.GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Acceleration);
    }

    public void DestroyComponent()
    {
        UpdateManager.Instance.RemoveElementUpdate(this);
        UpdateManager.Instance.RemoveElementFixedUpdate(this);
        Destroy(gameObject);
    }

    public WindBullet SetTarget(PlayerModel pl)
    {
        _target = pl;
        return this;
    }
}
