using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemy : Enemy
{
    public FireRing fireRingPF;
    public Transform fireRingSpawnPoint;

    public float ringSpeed;
    public float ringDuration;

    public override void Start()
    {
        base.Start();
        normal.FsmEnter += x =>
        {
            StartCoroutine(DelayedSendInputToFsm(doActionTime, "special"));
        };

        special.FsmEnter += x =>
        {
            StartCoroutine(ActiveAction(prepareActionTime, doActionTime));
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
        var fireRing = Instantiate(fireRingPF, fireRingSpawnPoint.position, transform.rotation);
        fireRing.SetSpeed(ringSpeed).SetDuration(ringDuration);
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
        //_playerModel.GetPower(_playerModel.!!Aca va la funcion del poder de fuego!!, (int)myPower);
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
}
