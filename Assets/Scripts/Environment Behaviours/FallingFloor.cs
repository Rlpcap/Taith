using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFloor : FallingObject, IIce, IMud, ICorrupt
{
    public FallingBridgeRB bridge;

    public GameObject iceTrigger, mudTrigger;

    float speed = 40;
    float amount = 0.03f;

    public float fallingTime;
    bool _almostFalling = false;

    bool _canBeDestroy = false;

    float plCurrentRadius;

    public float dissolveRadius;
    public bool CanBeDestroy
    {
        get { return _canBeDestroy; }
        set { _canBeDestroy = value; }
    }

    //public bool rotate = true;
    private float _speedRotationX;
    private float _speedRotationZ;

    public override void Start()
    {
        base.Start();
        _speedRotationX = UnityEngine.Random.Range(-1.5f, 1.5f);
        _speedRotationZ = UnityEngine.Random.Range(-1.5f, 1.5f);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (_almostFalling && !timeStopped)
            Shake();
        if (_falling && !timeStopped)
            RotateFloor();
    }

    private void RotateFloor()
    {
        Vector3 rotationDir = new Vector3(_speedRotationX * Time.deltaTime, 0, _speedRotationZ * Time.deltaTime);
        transform.Rotate(rotationDir);
    }

    private void Shake()
    {
        SoundManager.PlaySound(SoundManager.Sound.PlatformShake,transform.position);
        transform.position = new Vector3(transform.position.x + Mathf.Sin(Time.time * speed + .5f) * amount, transform.position.y + Mathf.Sin(Time.time * speed - .5f) * amount, transform.position.z + Mathf.Sin(Time.time * speed) * amount);
    }

    public void PlayerIceOn(Vector3 plPos, float plRadius)
    {
        plCurrentRadius = plRadius;
        foreach (var mat in GetComponent<Renderer>().materials)
        {
            mat.SetVector("_playerPos", plPos);
            mat.SetFloat("_playerRadius", plRadius);
        }
    }

    public void PlayerIceOff()
    {
        StartCoroutine(PlayerDissolve(0.1f));
    }

    IEnumerator PlayerDissolve(float dissolveSpeed)
    {
        var currentRadius = plCurrentRadius;
        while (currentRadius > 0)
        {
            currentRadius -= dissolveSpeed;
            foreach (var mat in GetComponent<Renderer>().materials)
            {
                //mat.SetFloat("_DissolveAmount", dissolveTime);
                mat.SetFloat("_playerRadius", currentRadius);
            }
            yield return null;
        }
    }

    public void IceOn(float lerp)
    {
        //Preguntar a Rafa
        //dissolveRadius = 17.5f;

        //if(iceTrigger)
        //    iceTrigger.SetActive(true);
        StopAllCoroutines();//Esto es por si usamos el mismo shader del enemigo para el poder del player.
        foreach (var mat in GetComponent<Renderer>().materials)
        {
            mat.SetFloat("_IceMudLerp", lerp);
            //mat.SetFloat("_DissolveAmount", dissolveTime);
            mat.SetFloat("_radius", dissolveRadius);
        }
    }

    public void IceOff()
    {
        //if(iceTrigger)
        //    iceTrigger.SetActive(false);
        StartCoroutine(DissolveMat(0.1f));
    }

    public void MudOn(float lerp)
    {
        if (mudTrigger)
            mudTrigger.SetActive(true);
        foreach (var mat in GetComponent<Renderer>().materials)
        {
            mat.SetFloat("_IceMudLerp", lerp);
            //mat.SetFloat("_DissolveAmount", dissolveTime);
            mat.SetFloat("_radius", dissolveRadius);
        }
    }

    public void MudOff()
    {
        if (mudTrigger)
            mudTrigger.SetActive(false);
        StartCoroutine(DissolveMat(0.1f));
    }

    public void CorruptionOn()
    {
        foreach (var mat in GetComponent<Renderer>().materials)
        {
            mat.SetFloat("_radius", dissolveRadius);
        }
    }

    public void CorruptionOff()
    {
        StartCoroutine(DissolveMat(0.2f));
    }

    public void UpdateCorruption(float r)
    {
        foreach (var mat in GetComponent<Renderer>().materials)
        {
            mat.SetFloat("_radius", r);
        }
    }

    public override void Freeze()
    {
        timeStopped = true;
        _falling = false;
    }

    public override void Unfreeze()
    {
        timeStopped = false;
        if (_hasToFall) _falling = true;
    }

    public override IEnumerator FreezeTime(float freezeTime)
    {
        Freeze();
        yield return UpdateManager.WaitForSecondsCustom(freezeTime);
        Unfreeze();
    }

    public IEnumerator StartFalling()
    {
        if(bridge) bridge.Push();

        _hasToFall = true;
        _almostFalling = true;
        StartCoroutine(LooseMagic(.02f));
        yield return UpdateManager.WaitForSecondsCustom(fallingTime);

        _almostFalling = false;
        if(!timeStopped) _falling = true;
    }

    private IEnumerator LooseMagic(float dissolveSpeed)
    {
        var currentDissolve = 1.5f;
        while (currentDissolve > -0.5f)
        {
            currentDissolve -= dissolveSpeed;
            foreach (var mat in GetComponent<Renderer>().materials)
            {
                //mat.SetFloat("_DissolveAmount", dissolveTime);
                mat.SetFloat("_DissolveAmount1", currentDissolve);
            }
            yield return null;
        }
    }

    public IEnumerator DissolveMat(float dissolveSpeed)
    {
        var currentRadius = dissolveRadius;
        while (currentRadius > 0)
        {
            currentRadius -= dissolveSpeed;
            foreach (var mat in GetComponent<Renderer>().materials)
            {
                //mat.SetFloat("_DissolveAmount", dissolveTime);
                mat.SetFloat("_radius", currentRadius);
            }
            yield return null;
        }
    }

    public FallingFloor SetDissolveRadius(float r)
    {
        dissolveRadius = r;
        return this;
    }

    public FallingFloor SetEnemyPos(Vector3 ep)
    {
        foreach (var mat in GetComponent<Renderer>().materials)
        {
            mat.SetVector("_enemyPos", ep);
        }
        return this;
    }

    //private void OnCollisionStay(Collision coll)
    //{
    //    if (coll.gameObject.layer == 10 && _canBeDestroy)
    //        Destroy(gameObject);
    //}
}
