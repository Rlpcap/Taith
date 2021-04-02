using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEnemy : MovingEnemy
{
    public float actionDuration;
    float dissolveTime = 0f;
    public GameObject mesh;
    public GameObject head;
    public GameObject head2;


    public override void Start()
    {
        base.Start();
        StartCoroutine(ActiveAction(doActionTime));
    }
    public override void Action()
    {
        StartCoroutine(SpeedUp());
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        canShoot = true;
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

    IEnumerator SpeedUp()
    {
        var normalSpeed = speed;
        var normalRotSpeed = rotSpeed;
        speed *= 3;
        rotSpeed *= 3;
        yield return new WaitForSeconds(actionDuration);
        speed = normalSpeed;
        rotSpeed = normalRotSpeed;
        StartCoroutine(ActiveAction(doActionTime));
    }

    //public override void GetHitEffect()
    //{
    //    _anim.SetTrigger("hit");
    //}

    public override void GetHitEffect()
    {
        base.GetHitEffect();
        _anim.SetTrigger("hit");
    }

    IEnumerator ActiveAction(float actionTime)
    {
        yield return new WaitForSeconds(actionTime);
        Action();
        yield return new WaitForSeconds(0.1f);
    }

}
