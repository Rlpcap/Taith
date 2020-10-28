using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class IceBullet : MonoBehaviour, IUpdate
{
    public float speed;
    public float plFreezeTime;

    public LayerMask playerLayer;
    public ParticleSystem hitPb;

    GameObject ignoreObject;

    void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);
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

        if (pl)
        {
            Instantiate(hitPb, transform.position, transform.rotation);
            pl.CallFreeze(plFreezeTime);
            UpdateManager.Instance.RemoveElementUpdate(this);
            Destroy(gameObject);
        }
    }
}
