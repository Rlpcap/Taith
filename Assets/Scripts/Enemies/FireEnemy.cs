using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemy : Enemy
{
    public FireRing fireRingPF;
    public Transform fireRingSpawnPoint;

    public float ringSpeed;
    public float ringDuration;
    public float ringSetOnFireDuration;

    public ParticleSystem head;

    public override void Start()
    {
        base.Start();

        _myPowerAction = _playerModel.Dash;

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
    }

    public void Shoot()
    {
        var fireRing = Instantiate(fireRingPF, fireRingSpawnPoint.position, transform.rotation);
        fireRing.SetSpeed(ringSpeed).SetDuration(ringDuration).SetOnFireDuration(ringSetOnFireDuration);
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
        //_playerModel.GetPower(_playerModel.Dash, (int)myPower); **ACA LE DOY EL PODER AL PLAYER**
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        _anim.SetTrigger("die");
        GetComponent<CapsuleCollider>().enabled = false;
        _RB.constraints = RigidbodyConstraints.FreezeAll;
        if(standingPlatform)
            transform.SetParent(standingPlatform.transform);

        while (dissolveTime < 1)
        {
            dissolveTime += 0.01f;
            mesh.GetComponent<Renderer>().materials[0].SetFloat("_DissolveAmount", dissolveTime);
            mesh.GetComponent<Renderer>().materials[1].SetFloat("_DissolveAmount", dissolveTime);
            yield return null;
        }
        head.Stop();
        yield return UpdateManager.WaitForSecondsCustom(1.08f);
        //yield return new WaitForSeconds(1.08f);
        //UpdateManager.Instance.RemoveElementPausable(this);
        //UpdateManager.Instance.RemoveElementUpdate(this);
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
}
