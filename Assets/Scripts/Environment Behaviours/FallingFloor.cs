using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFloor : FallingObject, IIce
{
    public FallingBridgeRB bridge;

    public GameObject iceTrigger;
    Color _originalColor;

    float speed = 40;
    float amount = 0.03f;

    public float fallingTime;
    bool _almostFalling = false;

    bool _canBeDestroy = false;
    public bool CanBeDestroy
    {
        get { return _canBeDestroy; }
        set { _canBeDestroy = value; }
    }

    private float _speedRotationX;
    private float _speedRotationZ;

    public override void Start()
    {
        base.Start();
        _speedRotationX = UnityEngine.Random.Range(-1f, 1f);
        _speedRotationZ = UnityEngine.Random.Range(-1f, 1f);
        _originalColor = GetComponent<MeshRenderer>().material.color;
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
        transform.position = new Vector3(transform.position.x + Mathf.Sin(Time.time * speed + .5f) * amount, transform.position.y + Mathf.Sin(Time.time * speed - .5f) * amount, transform.position.z + Mathf.Sin(Time.time * speed) * amount);
    }

    public void IceOn()
    {
        if(iceTrigger)
            iceTrigger.SetActive(true);
        GetComponent<MeshRenderer>().material.color = Color.cyan;
    }

    public void IceOff()
    {
        if(iceTrigger)
            iceTrigger.SetActive(false);
        GetComponent<MeshRenderer>().material.color = _originalColor;
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
        yield return new WaitForSeconds(freezeTime);
        Unfreeze();
    }

    public IEnumerator StartFalling()
    {
        if(bridge) bridge.Push();

        _hasToFall = true;
        _almostFalling = true;

        yield return new WaitForSeconds(fallingTime);

        _almostFalling = false;
        if(!timeStopped) _falling = true;
    }

    //private void OnCollisionStay(Collision coll)
    //{
    //    if (coll.gameObject.layer == 10 && _canBeDestroy)
    //        Destroy(gameObject);
    //}
}
