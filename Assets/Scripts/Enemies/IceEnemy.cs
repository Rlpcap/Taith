using MyFSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceEnemy : Enemy
{
    public float shootRange;
    public float iceRange;
    public float turnSpeed;
    public IceBullet iceBulletPF;
    public LayerMask ground;
    public Transform bulletSpawnPoint;
    public PhysicMaterial iceMat;
    public Material[] iceMats;

    public ParticleSystem feedbackAttack;

    //public List<ParticleSystem> listParticlesFeedbackCast;

    PlayerModel _target;

    Collider[] groundsAround;

    float dissolveTime = 0f;
    public GameObject mesh;
    public GameObject head;

    public override void Start()
    {
        base.Start();

        normal.FsmUpdate += () =>
        {
            if (!_isFrozen)
                LookAt();

            if (Vector3.Distance(transform.position, _playerModel.transform.position) < shootRange)
                SendInputToFSM("special");
        };

        special.FsmEnter += x =>
        {
            StartCoroutine(ActiveAction(prepareActionTime, doActionTime));
        };

        special.FsmUpdate += () =>
        {
            if (!_isFrozen)
                LookAt();
        };
        #region Por si falla la fsm
        //var idle = new State<string>("Idle");
        //var special = new State<string>("Special");
        //var hit = new State<string>("Hit");
        //var falling = new State<string>("Falling");
        //var die = new State<string>("Die");

        //StateConfigurer.Create(idle)
        //    .SetTransition("special", special)
        //    .SetTransition("hit", hit)
        //    .SetTransition("falling", falling)
        //    .Done();

        //StateConfigurer.Create(special)
        //    .SetTransition("idle", idle)
        //    .SetTransition("hit", hit)
        //    .SetTransition("falling", falling)
        //    .Done();

        //StateConfigurer.Create(hit)
        //    .SetTransition("idle", idle)
        //    .SetTransition("special", special)
        //    .SetTransition("falling", falling)
        //    .SetTransition("die", die)
        //    .Done();

        //StateConfigurer.Create(falling)
        //    .SetTransition("hit", hit)
        //    .Done();

        //idle.FsmUpdate += () =>
        //{
        //    CheckFalling();

        //    if (_falling)
        //        SendInputToFSM("falling");

        //    if (!_isFrozen)
        //        LookAt();

        //    if (Vector3.Distance(transform.position, _playerModel.transform.position) < shootRange)
        //        SendInputToFSM("special");
        //};

        //special.FsmEnter += x =>
        //{
        //    StartCoroutine(ActiveAction(prepareActionTime, doActionTime));
        //};

        //special.FsmUpdate += () =>
        //{
        //    CheckFalling();

        //    if (_falling)
        //        SendInputToFSM("falling");

        //    if (!_isFrozen)
        //        LookAt();
        //};

        //hit.FsmEnter += x =>
        //{
        //    Debug.Log("ouch");
        //    GetHitEffect();
        //    _currentHP -= 5;
        //    if (_currentHP <= 0)
        //        SendInputToFSM("die");
        //    else
        //        StartCoroutine(DelayedSendInputToFsm(stunnedTime, "idle"));
        //};

        //falling.FsmEnter += x =>
        //{
        //    StopAllCoroutines();
        //};

        //die.FsmEnter += x =>
        //{
        //    OnDeath();
        //};

        //_myFSM = new EventFSM<string>(idle);
        #endregion

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
        _myFSM.OnUpdate();
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
        //for (int i = 0; i < listParticlesFeedbackCast.Count-1; i++)
        //{
        //    listParticlesFeedbackCast[i].Play();
        //}
        //_anim.SetTrigger("startCasting");
    }

    public override void Action()
    {
        _anim.SetTrigger("shoot");
        //StartCoroutine(ActiveAction(prepareActionTime, doActionTime));
    }

    public void Shoot()
    {
        var iceBullet = Instantiate(iceBulletPF, bulletSpawnPoint.position, transform.rotation);
        iceBullet.GetIgnore(gameObject);
        //for (int i = 0; i < listParticlesFeedbackCast.Count - 1; i++)
        //{
        //    listParticlesFeedbackCast[i].Stop();
        //}
        SendInputToFSM("normal");
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
        _anim.SetTrigger("hit");
    }

    IEnumerator Die()
    {
        _anim.SetTrigger("die");
        while (dissolveTime < 1)
        {
            dissolveTime += 0.01f;
            mesh.GetComponent<Renderer>().materials[0].SetFloat("_DissolveAmount", dissolveTime);
            mesh.GetComponent<Renderer>().materials[1].SetFloat("_DissolveAmount", dissolveTime);
            mesh.GetComponent<Renderer>().materials[2].SetFloat("_DissolveAmount", dissolveTime);
            mesh.GetComponent<Renderer>().materials[3].SetFloat("_DissolveAmount", dissolveTime);
            yield return null;
        }
        head.GetComponentInChildren<ParticleSystem>().Stop();
        GetComponent<CapsuleCollider>().enabled = false;
        yield return new WaitForSeconds(1.08f);
        UpdateManager.Instance.RemoveElementUpdate(this);
        Destroy(gameObject);
    }
    IEnumerator ActiveAction(float feedbackTime, float actionTime)
    {
        yield return new WaitForSeconds(feedbackTime);
        FeedbackAction();
        yield return new WaitForSeconds(actionTime);
        Action();
        yield return new WaitForSeconds(0.1f);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}
