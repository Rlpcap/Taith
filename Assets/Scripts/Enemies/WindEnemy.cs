using MyFSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEnemy : Enemy
{
    //public float range;
    //public float turnSpeed;
    //public float timeToSpawn;
    public WindBullet wind;
    bool _isAttacking;
    public ParticleSystem feedBackAttack;
    WindShaderController _windMat;
    bool _windPlaying;
    //public Material dissolve;
    float dissolveTime = 0f;
    public GameObject mesh;
    public GameObject head;


    public override void Start()
    {
        base.Start();
        _windMat = wind.GetComponentInChildren<WindShaderController>();
        //StartCoroutine(ActiveAction(prepareActionTime, doActionTime));

        normal.FsmEnter += x =>
        {
            StartCoroutine(DelayedSendInputToFsm(doActionTime, "special"));
        };

        normal.FsmUpdate += () =>
        {
            if (wind)
                TurnWind();
        };

        special.FsmEnter += x =>
        {
            StartCoroutine(ActiveAction(prepareActionTime, doActionTime));
        };

        special.FsmUpdate += () =>
        {
            if (wind)
                TurnWind();
        };

        falling.FsmEnter += x =>
        {
            _isAttacking = false;
        };

        normal.Enter(_myFSM.Current.Name);
        //StartCoroutine(DelayedSendInputToFsm(doActionTime, "special"));
    }

    public override void OnUpdate()
    {
        //base.OnUpdate();
        //if (wind)
        //    TurnWind();

        //if (_falling)
        //    _isAttacking = false;

        _myFSM.OnUpdate();
    }

    private void TurnWind()
    {
        if (_isAttacking)
        {
            wind.gameObject.SetActive(true);
            if (!_windPlaying)
            {
                _windPlaying = true;
                _windMat.CallStepWind();
            }
        }
        else
        {
            wind.gameObject.SetActive(false);
            wind.useWind = false;
            _windPlaying = false;
        }
    }

    public override void FeedbackAction()
    {
        if(!_isAttacking)
        {
            //_anim.SetTrigger("startCasting");
            feedBackAttack.Play();
        }
    }

    public override void Action()
    {
        _isAttacking = CheckIfAttacking(_isAttacking);
        TurnWind();
        if(_isAttacking)
            _anim.SetTrigger("shoot");

        SendInputToFSM("normal");
        //StartCoroutine(ActiveAction(prepareActionTime, doActionTime));
    }

    bool CheckIfAttacking(bool a)
    {
        if (a)
            return false;
        else
            return true;
    }
    public override void OnDeath()
    {
        StopAllCoroutines();
        wind.useWind = false;
        wind.DestroyComponent();
        _playerModel.GetPower(_playerModel.SuperJump, (int)myPower);
        //_playerModel.ActivePower = _playerModel.SuperJump;
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        _anim.SetTrigger("die");
        GetComponent<CapsuleCollider>().enabled = false;
        while (dissolveTime < 1)
        {
            dissolveTime += 0.01f;
            mesh.GetComponent<Renderer>().materials[0].SetFloat("_DissolveAmount", dissolveTime);
            mesh.GetComponent<Renderer>().materials[2].SetFloat("_DissolveAmount", dissolveTime);
            yield return null;
        }
        head.GetComponentInChildren<ParticleSystem>().Stop();
        yield return new WaitForSeconds(1.08f);
        UpdateManager.Instance.RemoveElementUpdate(this);
        Destroy(gameObject);
    }

    public override void GetHitEffect()
    {
        _anim.SetTrigger("hit");
    }

    IEnumerator ActiveAction(float feedbackTime, float actionTime)
    {
        yield return new WaitForSeconds(feedbackTime);
        FeedbackAction();
        yield return new WaitForSeconds(actionTime);
        Action();
        yield return new WaitForSeconds(0.1f);
    }
}
