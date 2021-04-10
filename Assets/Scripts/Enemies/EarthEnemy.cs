using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthEnemy : Enemy
{
    public EarthWall earthWallPF;
    public Transform earthWallSpawnPoint;

    public override void Start()
    {
        base.Start();

        normal.FsmEnter += x =>
        {
            DelayedSendInputToFsm(doActionTime, "special");
        };

        special.FsmEnter += x =>
        {
            ActiveAction(prepareActionTime, doActionTime);
        };

        normal.Enter(_myFSM.Current.Name);
    }

    public override void Action()
    {
        _anim.SetTrigger("shoot");
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

}
