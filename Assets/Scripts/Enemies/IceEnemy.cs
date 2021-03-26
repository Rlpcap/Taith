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

    public List<ParticleSystem> listParticlesFeedbackCast;

    PlayerModel _target;

    Collider[] groundsAround;

    float dissolveTime = 0f;
    public GameObject mesh;
    public GameObject mesh1;
    public GameObject head;

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
        for (int i = 0; i < listParticlesFeedbackCast.Count-1; i++)
        {
            listParticlesFeedbackCast[i].Play();
        }
        _anim.SetTrigger("startCasting");
    }

    public override void Action()
    {
        _anim.SetTrigger("shoot");
        var iceBullet = Instantiate(iceBulletPF, bulletSpawnPoint.position, transform.rotation);
        iceBullet.GetIgnore(gameObject);
        for (int i = 0; i < listParticlesFeedbackCast.Count-1; i++)
        {
            listParticlesFeedbackCast[i].Stop();
        }
    }

    public override void OnDeath()
    {
        StopAllCoroutines();
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
        StartCoroutine(Die());
        
    }

    public override void GetHitEffect()
    {

    }

    IEnumerator Die()
    {
        _anim.SetTrigger("die");
        while (dissolveTime < 1)
        {
            dissolveTime += 0.01f;
            mesh.GetComponent<Renderer>().materials[0].SetFloat("_DissolveAmount", dissolveTime);
            mesh1.GetComponent<Renderer>().materials[0].SetFloat("_DissolveAmount", dissolveTime);
            yield return null;
        }
        head.GetComponentInChildren<ParticleSystem>().Stop();
        yield return new WaitForSeconds(1.08f);
        UpdateManager.Instance.RemoveElementUpdate(this);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}
