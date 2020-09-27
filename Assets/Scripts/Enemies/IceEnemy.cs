using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class IceEnemy : Enemy
{
    public float iceRange;
    public float turnSpeed;
    public IceBullet iceBulletPF;
    public LayerMask ground;
    public Transform bulletSpawnPoint;

    PlayerModel _target;

    Collider[] groundsAround;

    public override void Start()
    {
        base.Start();
        groundsAround = Physics.OverlapSphere(transform.position, iceRange, ground);
        foreach (var ground in groundsAround)
        {
            if (ground.GetComponent<IIce>() != null)
                ground.GetComponent<IIce>().IceOn();
        }
        _target = FindObjectOfType<PlayerModel>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        LookAt();
    }

    void LookAt()
    {
        var nextForward = (_target.transform.position - transform.position).normalized;
        nextForward.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, nextForward, turnSpeed);
    }

    public override void Action()
    {
        var iceBullet = Instantiate(iceBulletPF, bulletSpawnPoint.position, transform.rotation);
        iceBullet.GetIgnore(this.gameObject);
    }

    public override void OnDeath()
    {
        foreach (var ground in groundsAround)
        {
            if (ground.GetComponent<IIce>() != null)
                ground.GetComponent<IIce>().IceOff();
        }
        _target.OnIce = false;
        UpdateManager.Instance.RemoveElementUpdate(this);
        Destroy(gameObject);
    }
}
