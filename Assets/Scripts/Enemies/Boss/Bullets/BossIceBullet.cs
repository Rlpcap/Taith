using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIceBullet : BossBullet, IPrototype
{
    Vector3 _dir;
    bool _copy;
    public float copySizeDivide;
    public float copyLifeTime;

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
        var b1 = Instantiate(this);
        b1.transform.position += new Vector3(0, .5f, 0);
        b1.SetDir(transform.forward).SetBool(true).SetSize(transform.localScale.x / copySizeDivide).SetLifeTime(copyLifeTime);

        var b2 = Instantiate(this);
        b2.transform.position += new Vector3(0, .5f, 0);
        b2.SetDir(transform.right).SetBool(true).SetSize(transform.localScale.x / copySizeDivide).SetLifeTime(copyLifeTime);

        var b3 = Instantiate(this);
        b3.transform.position += new Vector3(0, .5f, 0);
        b3.SetDir(-transform.forward).SetBool(true).SetSize(transform.localScale.x / copySizeDivide).SetLifeTime(copyLifeTime);

        var b4 = Instantiate(this);
        b4.transform.position += new Vector3(0, .5f, 0);
        b4.SetDir(-transform.right).SetBool(true).SetSize(transform.localScale.x / copySizeDivide).SetLifeTime(copyLifeTime);

        return b1;
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
            if(!_copy)
                Clone();
            StopAllCoroutines();
            StartCoroutine(Die(.1f));
        }
    }

    public BossIceBullet SetLifeTime(float l)
    {
        lifeTime = l;
        return this;
    }

    public BossIceBullet SetSpeed(float s)
    {
        speed = s;
        return this;
    }

    public BossIceBullet SetDir(Vector3 d)
    {
        _dir = d;
        return this;
    }

    public BossIceBullet SetBool(bool c)
    {
        _copy = c;
        return this;
    }

    public BossIceBullet SetSize(float s)
    {
        transform.localScale = new Vector3(s, s, s);
        return this;
    }
}
