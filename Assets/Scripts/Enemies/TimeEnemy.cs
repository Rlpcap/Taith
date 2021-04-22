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
    float _currentSpeed;
    public float rotSpeedNormal;
    public float rotSpeedSpecial;
    float _currentRotSpeed;
    public List<Transform> waypoints = new List<Transform>();
    protected int _index = 0;
    public ParticleSystem dustTrail;


    public override void Start()
    {
        base.Start();
        _currentSpeed = speedNormal;
        _currentRotSpeed = rotSpeedNormal;

        normal.FsmEnter += x =>
        {
            _currentSpeed = speedNormal;
            _currentRotSpeed = rotSpeedNormal;
            StartCoroutine(ActiveAction(doActionTime));
        };

        normal.FsmUpdate += () =>
        {
            Move();
        };

        special.FsmEnter += x =>
        {
            _currentSpeed = speedSpecial;
            _currentRotSpeed = rotSpeedSpecial;
            dustTrail.Play();
            StartCoroutine(DelayedSendInputToFsm(doActionTime, "normal"));
        };

        special.FsmUpdate += () =>
        {
            Move();
        };

        special.FsmExit += x =>
        {
            dustTrail.Stop();
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

        _playerModel.GetPower(_playerModel.StopTime, (int)myPower);
        //_playerModel.ActivePower = _playerModel.StopTime;
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
        head2.GetComponentInChildren<ParticleSystem>().Stop();
        yield return new WaitForSeconds(1.08f);
        UpdateManager.Instance.RemoveElementUpdate(this);
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
        yield return new WaitForSeconds(actionTime);
        Action();
        yield return new WaitForSeconds(0.1f);
    }

}
