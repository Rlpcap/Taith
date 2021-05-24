using MyFSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthEnemy : Enemy
{
    public EarthWall earthWallPF;
    public Transform earthWallSpawnPoint;
    public GameObject earthShield;
    PlayerModel _target;
    public float shieldRange;
    public float wallSpeed;
    public float wallDuration;

    State<string> shield;


    bool _shielded = false;
    bool _brokenShield = false;

    public GameObject head;

    public LayerMask ground;
    public float mudRange;

    Collider[] groundsAround;

    public override void Start()
    {
        base.Start();

        _myPowerAction = _playerModel.EarthShield;
        groundsAround = Physics.OverlapSphere(transform.position, mudRange, ground);
        foreach (var ground in groundsAround)
        {
            if (ground.GetComponent<IMud>() != null)
            {
                ground.GetComponent<FallingFloor>().SetDissolveRadius(mudRange + 2);
                ground.GetComponent<Renderer>().material.SetVector("_enemyPos", transform.position);
                ground.GetComponent<IMud>().MudOn(1);
            }
        }

        _target = FindObjectOfType<PlayerModel>();

        shield = new State<string>("Shield");

        normal.AddTransition("shield", shield);

        StateConfigurer.Create(shield)
            .SetTransition("normal", normal)
            .Done();

        normal.FsmEnter += x =>
        {
            StartCoroutine(DelayedSendInputToFsm(doActionTime, "special"));
        };

        normal.FsmUpdate += () =>
        {
            if (Vector3.Distance(transform.position, _playerModel.transform.position) < shieldRange && !_brokenShield)
                SendInputToFSM("shield");
        };

        special.FsmEnter += x =>
        {
            StartCoroutine(ActiveAction(prepareActionTime, doActionTime));
        };

        special.FsmUpdate += () =>
        {
            if (Vector3.Distance(transform.position, _playerModel.transform.position) < shieldRange && !_brokenShield)
                SendInputToFSM("shield");
        };

        shield.FsmEnter += x =>
        {
            StopAllCoroutines();
            earthShield.SetActive(true);
            //Crear el escudo
            _shielded = true;
        };

        shield.FsmExit += x =>
        {
            //Romper el escudo (lo estoy rompiendo en la funcion de ontriggerenter cuando me atacan).
            _shielded = false;
            _brokenShield = true;
        };

        normal.Enter(_myFSM.Current.Name);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        _myFSM.OnUpdate();
    }

    public override void Action()
    {
        _anim.SetTrigger("shoot");
    }

    public void Shoot()
    {
        var earthWall = Instantiate(earthWallPF, earthWallSpawnPoint.position, transform.rotation);
        earthWall.SetOwner(this).SetDuration(wallDuration).SetSpeed(wallSpeed);
        SendInputToFSM("normal");
    }

    public override void FeedbackAction()
    {
    }

    public override void GetHitEffect()
    {
        _anim.SetTrigger("hit");
    }

    public override void OnDeath()
    {
        StopAllCoroutines();

        foreach (var ground in groundsAround)
        {
            if (ground.GetComponent<IMud>() != null)
                ground.GetComponent<IMud>().MudOff();
            //ground.GetComponent<Collider>().material = null;
        }
        _target.UnMud();

        //_playerModel.GetPower(_playerModel.EarthShield, (int)myPower); **ACA LE DOY EL PODER AL PLAYER**
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        _anim.SetTrigger("die");
        GetComponent<CapsuleCollider>().enabled = false;
        _RB.constraints = RigidbodyConstraints.FreezeAll;
        transform.SetParent(standingPlatform.transform);

        while (dissolveTime < 1)
        {
            dissolveTime += 0.01f;
            mesh.GetComponent<Renderer>().materials[0].SetFloat("_DissolveAmount", dissolveTime);
            mesh.GetComponent<Renderer>().materials[1].SetFloat("_DissolveAmount", dissolveTime);
            yield return null;
        }
        head.GetComponentInChildren<ParticleSystem>().Stop();

        yield return UpdateManager.WaitForSecondsCustom(1.08f);
        //yield return new WaitForSeconds(1.08f);

        UpdateManager.Instance.RemoveElementUpdate(this);
        Destroy(gameObject);
    }

    IEnumerator ActiveAction(float feedbackTime, float actionTime)
    {
        yield return UpdateManager.WaitForSecondsCustom(feedbackTime);
        //yield return new WaitForSeconds(feedbackTime);
        FeedbackAction();
        yield return UpdateManager.WaitForSecondsCustom(actionTime);
        //yield return new WaitForSeconds(actionTime);
        Action();
        yield return UpdateManager.WaitForSecondsCustom(0.1f);
        //yield return new WaitForSeconds(0.1f);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "MeleeCollider" && !_isDead && !_shielded)
        {
            other.gameObject.SetActive(false);
            SendInputToFSM("hit");
        }
        else if(other.gameObject.name == "MeleeCollider" && _shielded)
        {
            earthShield.SetActive(false);
            StartCoroutine(DelayedSendInputToFsm(stunnedTime, "normal"));
        }
    }
}
