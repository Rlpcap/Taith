﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class IceBullet : MonoBehaviour, IUpdate
{
    public float speed;
    public float lifeTime;
    public float plFreezeTime;

    public LayerMask playerLayer;
    public ParticleSystem hitPb;

    GameObject ignoreObject;

    void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);
        StartCoroutine(Die(lifeTime));
    }

    public void OnUpdate()
    {
        Move();
    }

    void Move()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public IceBullet GetIgnore(GameObject obj)
    {
        ignoreObject = obj;
        return this;
    }

    private void OnTriggerEnter(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();
        var pv = coll.GetComponent<PlayerView>();

        if (pl)
        {
            Instantiate(hitPb, transform.position, transform.rotation);
            pl.CallFreeze(plFreezeTime,pv.onFreeze);
            StopAllCoroutines();
            UpdateManager.Instance.RemoveElementUpdate(this);
            Destroy(gameObject);
        }
    }

    IEnumerator Die(float time)
    {
        yield return new WaitForSeconds(time);
        UpdateManager.Instance.RemoveElementUpdate(this);
        Destroy(gameObject);
    }
}
