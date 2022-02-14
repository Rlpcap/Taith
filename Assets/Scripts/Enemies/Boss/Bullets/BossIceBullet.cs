using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIceBullet : BossBullet, IPrototype
{
    Vector3 _dir;

    protected override void Start()
    {
        base.Start();
        _dir = Vector3.down;
    }

    public override void OnUpdate()
    {
        Move();
    }

    void Move()
    {
        transform.position += _dir * speed * Time.deltaTime;
    }


    public IPrototype Clone()
    {
        var b = Instantiate(this);
        b.SetDir(transform.up);

        return b;
    }



    private void OnTriggerEnter(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();
        if (pl)
        {
            if (!pl.Shielded)
                pl.CallFreeze(1);

            StopAllCoroutines();
            UpdateManager.Instance.RemoveElementUpdate(this);
            Destroy(gameObject);
        }
        else if (coll.gameObject.layer == 9)
        {
            //var f = Instantiate(spawnFloor, transform.position, transform.rotation);
            Clone();
            StopAllCoroutines();
            UpdateManager.Instance.RemoveElementUpdate(this);
            Destroy(gameObject);
        }
    }

    BossIceBullet SetLifeTime(float l)
    {
        lifeTime = l;
        return this;
    }

    BossIceBullet SetSpeed(float s)
    {
        speed = s;
        return this;
    }

    BossIceBullet SetDir(Vector3 d)
    {
        _dir = d;
        return this;
    }
}
