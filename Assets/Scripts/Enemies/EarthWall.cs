﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthWall : MonoBehaviour, IUpdate, IFreezable
{

    public float speed;

    public float lifeTime;
    public float hitDuration;

    bool _canMove;
    EarthEnemy _owner;

    IEnumerator _destroyGo;
    IEnumerator _destroyGoLifetime;

    void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);

        _destroyGo = DestroyGO(hitDuration);

        _destroyGoLifetime = DestroyGO(lifeTime);
        StartCoroutine(_destroyGoLifetime);

        _canMove = true;
    }

    public void OnUpdate()
    {
        Move();
    }

    void Move()
    {
        if (_canMove)
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    IEnumerator DestroyGO(float time)
    {
        yield return UpdateManager.WaitForSecondsCustom(time);
        //yield return new WaitForSeconds(time);

        if(gameObject!=null)
        {
            UpdateManager.Instance.RemoveElementUpdate(this);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        var bullet = coll.gameObject.GetComponent<EarthWall>();
        if(bullet)
        {
            Debug.Log("DESTROY WALL");
            StopAllCoroutines();
            UpdateManager.Instance.RemoveElementUpdate(this);
            Destroy(gameObject);
        }

        var player = coll.gameObject.GetComponent<PlayerModel>();
        if (player)
        {
            Debug.Log("HIT PLAYER");
            StopCoroutine(_destroyGoLifetime);
            StartCoroutine(_destroyGo);
        }


    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("EarthShield"))
        {
            StopAllCoroutines();
            UpdateManager.Instance.RemoveElementUpdate(this);
            Destroy(gameObject);
        }
    }

    public EarthWall SetDuration(float dur)
    {
        lifeTime = dur;
        return this;
    }

    public EarthWall SetSpeed(float spd)
    {
        speed = spd;
        return this;
    }

    public EarthWall SetOwner(EarthEnemy owner)
    {
        _owner = owner;
        return this;
    }

    public void Freeze()
    {
        _canMove = false;
        StopCoroutine(_destroyGoLifetime);

    }

    public void Unfreeze()
    {
        _canMove = true;

        StartCoroutine(_destroyGoLifetime);
    }

    public IEnumerator FreezeTime(float f)
    {
        Freeze();
        yield return new WaitForSeconds(f);

        if(this!=null)
        Unfreeze();
    }
}
