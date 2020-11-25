using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceEnemy : Enemy
{
    public float iceRange;
    public float turnSpeed;
    public IceBullet iceBulletPF;
    public LayerMask ground;
    public Transform bulletSpawnPoint;
    public PhysicMaterial iceMat;
    public Material[] iceMats;

    public ParticleSystem feedbackAttack;

    PlayerModel _target;

    Collider[] groundsAround;

    public override void Start()
    {
        base.Start();

        groundsAround = Physics.OverlapSphere(transform.position, iceRange, ground);
        foreach (var ground in groundsAround)
        {
            if (ground.GetComponent<IIce>() != null)
            {
                ground.GetComponent<IIce>().IceOn();
                ground.GetComponent<Collider>().material = iceMat;
                ground.GetComponent<Renderer>().materials = iceMats;
                iceMats[0].SetFloat("_alpha", 0.7f);
                iceMats[1].SetFloat("_alpha", 0.7f);
                iceMats[2].SetFloat("_alpha", 0.3f);
            }
        }
        _target = FindObjectOfType<PlayerModel>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if(!_falling && !_isFreezed)
            LookAt();
    }

    void LookAt()
    {
        var nextForward = (_target.transform.position - transform.position).normalized;
        nextForward.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, nextForward, turnSpeed);
    }

    public override void FeedbackAction()
    {
        feedbackAttack.Play();
        _anim.SetTrigger("startCasting");
    }

    public override void Action()
    {
        _anim.SetTrigger("shoot");
        var iceBullet = Instantiate(iceBulletPF, bulletSpawnPoint.position, transform.rotation);
        iceBullet.GetIgnore(gameObject);
    }

    public override void OnDeath()
    {
        for (int i = 0; i < iceMats.Length; i++)
        {
            iceMats[i].SetFloat("_alpha", 0f);
        }

        foreach (var ground in groundsAround)
        {
            if (ground.GetComponent<IIce>() != null)
                ground.GetComponent<IIce>().IceOff();
            ground.GetComponent<Collider>().material = null;
        }
        _target.OnIce = false;
        _target.GetPower(_target.IceLaser, (int)myPower);
        //_target.ActivePower = _target.IceLaser;
        UpdateManager.Instance.RemoveElementUpdate(this);
        Destroy(gameObject);
    }

    public override void GetHitEffect()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}
