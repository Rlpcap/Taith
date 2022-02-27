using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEnemy : Enemy
{
    public float actionDuration;
    public GameObject head;
    public GameObject head2;
    public float speedNormal;
    public float speedSpecial;
    public float rotSpeedNormal;
    public float rotSpeedSpecial;
    public float ammountStunTime;
    public List<Transform> waypoints = new List<Transform>();
    public ParticleSystem dustTrail;

    protected int _index = 0;

    float _currentSpeed;
    float _currentRotSpeed;
    bool _isAttacking;

    public TimePuddle timePuddle;

    BoxCollider _hitStunCollider;

    public override void Start()
    {
        base.Start();
        _hitStunCollider = GetComponent<BoxCollider>();
        _myPowerAction = _playerModel.StopTime;
        _currentSpeed = speedNormal;
        _currentRotSpeed = rotSpeedNormal;


        timePuddle.SetMaxRadius(4);
        timePuddle.GetComponent<Renderer>().material.SetFloat("_radius", 0);

        normal.FsmEnter += x =>
        {
            _currentSpeed = speedNormal;
            _currentRotSpeed = rotSpeedNormal;
            _isAttacking = false;
            StartCoroutine(ActiveAction(doActionTime));
        };

        normal.FsmUpdate += () =>
        {
            Move();
        };

        special.FsmEnter += x =>
        {
            timePuddle.TimeOn();
            _hitStunCollider.enabled = true;
            _currentSpeed = speedSpecial;
            _currentRotSpeed = rotSpeedSpecial;
            dustTrail.Play();
            _isAttacking = true;
            StartCoroutine(DelayedSendInputToFsm(doActionTime, "normal"));
        };

        special.FsmUpdate += () =>
        {
            Move();
        };

        special.FsmExit += x =>
        {
            _hitStunCollider.enabled = false;
            timePuddle.TimeOff();
            dustTrail.Stop();
            _isAttacking = false;
        };

        StartCoroutine(ActiveAction(doActionTime));
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        _myFSM.OnUpdate();
    }

    private void Move()
    {
        if (Vector3.Distance(transform.position, waypoints[_index].position) < 1.5f)
        {
            _index++;
            if (_index > waypoints.Count - 1)
            {
                _index = 0;
            }
        }
        Vector3 dir = (waypoints[_index].position - transform.position).normalized;
        dir.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * _currentRotSpeed);
        transform.position += transform.forward * _currentSpeed * Time.deltaTime;
    }

    public override void Action()
    {
        //StartCoroutine(SpeedUp());
        SendInputToFSM("special");
    }

    public override void FeedbackAction()
    {

    }

    public override void OnDeath()
    {
        StopAllCoroutines();
        //_playerModel.CanFreezeTime = true;

        //_playerModel.GetPower(_playerModel.StopTime, (int)myPower); **ACA LE DOY EL PODER AL PLAYER**
        //_playerModel.ActivePower = _playerModel.StopTime;
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        _anim.SetTrigger("die");
        GetComponent<CapsuleCollider>().enabled = false;
        _RB.constraints = RigidbodyConstraints.FreezeAll;
        if (standingPlatform)
            transform.SetParent(standingPlatform.transform);

        while (dissolveTime < 1)
        {
            dissolveTime += 0.01f;
            mesh.GetComponent<Renderer>().materials[0].SetFloat("_DissolveAmount", dissolveTime);
            mesh.GetComponent<Renderer>().materials[1].SetFloat("_DissolveAmount", dissolveTime);
            yield return null;
        }
        head.GetComponentInChildren<ParticleSystem>().Stop();
        head2.GetComponentInChildren<ParticleSystem>().Stop();
        yield return UpdateManager.WaitForSecondsCustom(1.08f);
        //yield return new WaitForSeconds(1.08f);
        //UpdateManager.Instance.RemoveElementPausable(this);
        //UpdateManager.Instance.RemoveElementUpdate(this);
        Destroy(gameObject);
    }

    //IEnumerator SpeedUp()
    //{
    //    var normalSpeed = this.speedNormal;
    //    var normalRotSpeed = rotSpeed;
    //    this.speedNormal *= 3;
    //    rotSpeed *= 3;
    //    yield return new WaitForSeconds(actionDuration);
    //    this.speedNormal = normalSpeed;
    //    rotSpeed = normalRotSpeed;
    //    StartCoroutine(ActiveAction(doActionTime));
    //}

    public override void GetHitEffect()
    {
        _anim.SetTrigger("hit");
    }

    IEnumerator ActiveAction(float actionTime)
    {
        yield return UpdateManager.WaitForSecondsCustom(actionTime);
        Action();
        yield return UpdateManager.WaitForSecondsCustom(0.1f);
    }


    private void OnCollisionEnter(Collision collision)
    {
        //  NOSE SI QUEDARIA MEJOR QUE ACCEDA A ESTA FUNCION DESDE OTRO LADO COMO POR EJEMPLO DEL PLAYER
        //  DE ESTA MANERA FUNCIONA PERO AL LLAMAR AL CALL FREEZE PRENDE EL SHADER DE CONGELADO. QUERIA VER SI PODIA LLAMAR A ESTO PERO QUE REPRODUZCA AL SHADER QUE CORRESPONDE SIN-
        //  ANDAR HACIENDO OTRO METODO QUE REALIZA LA MISMA FUNCION QUE EL FREEZE. PARA APROVECHAR TODO MEJOR

        //if(_playerModel)
        //{
        //    if(_isAttacking && !_playerModel.Shielded)
        //    {
        //        _playerModel.CallStopInTime(ammountStunTime);
        //    }
        //}
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        var pl = other.GetComponent<PlayerModel>();
        if (pl)
        {
            if (_isAttacking && !_playerModel.Shielded)
            {
                _playerModel.CallStopInTime(ammountStunTime);
            }
        }
    }
}
