using MyFSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthEnemy : Enemy
{
    public EarthWall earthWallPF;
    public Transform earthWallSpawnPoint;
    PlayerModel _target;
    public float shieldRange;

    State<string> shield;

    bool _shielded = false;
    bool _brokenShield = false;

    public override void Start()
    {
        base.Start();
        _target = FindObjectOfType<PlayerModel>();

        shield = new State<string>("Shield");

        normal.AddTransition("shield", shield);

        StateConfigurer.Create(shield)
            .SetTransition("normal", normal)
            .Done();

        normal.FsmEnter += x =>
        {
            StartCoroutine(DelayedSendInputToFsm(doActionTime, "special"));
            Debug.Log("normal");
        };

        normal.FsmUpdate += () =>
        {
            if (Vector3.Distance(transform.position, _playerModel.transform.position) < shieldRange && !_brokenShield)
                SendInputToFSM("shield");
        };

        special.FsmEnter += x =>
        {
            StartCoroutine(ActiveAction(prepareActionTime, doActionTime));
            Debug.Log("special");
        };

        special.FsmUpdate += () =>
        {
            if (Vector3.Distance(transform.position, _playerModel.transform.position) < shieldRange && !_brokenShield)
                SendInputToFSM("shield");
        };

        shield.FsmEnter += x =>
        {
            StopAllCoroutines();
            Debug.Log("Me cubro");
            //Crear el escudo
            _shielded = true;
        };

        shield.FsmExit += x =>
        {
            //Romper el escudo
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
        Shoot();//Sacar esto cuando esté la animación de shoot
    }

    public void Shoot()
    {
        var earthWall = Instantiate(earthWallPF, earthWallSpawnPoint.position, transform.rotation);
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
        //_playerModel.GetPower(_playerModel.!!Aca va la funcion del poder de tierra!!, (int)myPower);
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        _anim.SetTrigger("die");
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

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "MeleeCollider" && !_isDead && !_shielded)
        {
            other.gameObject.SetActive(false);
            SendInputToFSM("hit");
        }
        else if(other.gameObject.name == "MeleeCollider" && _shielded)
        {
            StartCoroutine(DelayedSendInputToFsm(stunnedTime, "normal"));
        }
    }
}
