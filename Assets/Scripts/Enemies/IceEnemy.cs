using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceEnemy : Enemy
{
    public float iceRange;
    public float turnSpeed;
    public LayerMask ground;
    PlayerModel _target;

    void Start()
    {
        var nearbyGrounds = Physics.OverlapSphere(transform.position, iceRange, ground);
        foreach (var ground in nearbyGrounds)
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
        transform.forward = Vector3.Lerp(transform.forward, nextForward, turnSpeed);
    }

    public override void Action()
    {
        Debug.Log("Te tire alta bola de hielo");
    }

    public override void OnDeath()
    {
        var nearbyGrounds = Physics.OverlapSphere(transform.position, iceRange, ground);
        foreach (var ground in nearbyGrounds)
        {
            if (ground.GetComponent<IIce>() != null)
                ground.GetComponent<IIce>().IceOff();
        }
    }
}
