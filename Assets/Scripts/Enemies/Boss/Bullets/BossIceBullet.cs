using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIceBullet : BossBullet, IPrototype
{
    Vector3 _dir;
    bool _copy;
    public float copyLifeTime;
    public float copySizeDivideMin;
    public float copySizeDivideMax;
    public float copySpeedMin;
    public float copySpeedMax;

    public GameObject copyBulletEffect;

    PlayerModel _target;

    protected override void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);

        if (!_copy)
        {
            _dir = Vector3.down;
            StartCoroutine(Prepare(_prepareTime));
            SoundManager.PlaySound(SoundManager.Sound.BossIceBall, transform.position);
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponentInChildren<ParticleSystem>().Stop();
            copyBulletEffect.SetActive(true);
            StartCoroutine(Die(lifeTime));
        }
    }

    public override void OnUpdate()
    {
        Move();
    }

    void Move()
    {
        if(_started || _copy)
            transform.position += _dir * speed * Time.deltaTime;
    }

    public IPrototype Clone()
    {
        var sizeRandom = Random.Range(copySizeDivideMin, copySizeDivideMax);
        var speedRandom = Random.Range(copySpeedMin, copySpeedMax);

        int dirRandom = Random.Range(0, 2);

        if(dirRandom == 0)
        {
            var b1 = Instantiate(this);
            b1.transform.position += new Vector3(0, .5f, 0);
            b1.SetDir(transform.forward).SetBool(true).SetSize(transform.localScale.x / sizeRandom).SetLifeTime(copyLifeTime).SetSpeed(speedRandom);

            var b2 = Instantiate(this);
            b2.transform.position += new Vector3(0, .5f, 0);
            b2.transform.forward = -transform.forward;
            b2.SetDir(-transform.forward).SetBool(true).SetSize(transform.localScale.x / sizeRandom).SetLifeTime(copyLifeTime).SetSpeed(speedRandom);

            return b1;
        }
        else
        {
            var b3 = Instantiate(this);
            b3.transform.position += new Vector3(0, .5f, 0);
            b3.transform.forward = transform.right;
            b3.SetDir(transform.right).SetBool(true).SetSize(transform.localScale.x / sizeRandom).SetLifeTime(copyLifeTime).SetSpeed(speedRandom);

            var b4 = Instantiate(this);
            b4.transform.position += new Vector3(0, .5f, 0);
            b4.transform.forward = -transform.right;
            b4.SetDir(-transform.right).SetBool(true).SetSize(transform.localScale.x / sizeRandom).SetLifeTime(copyLifeTime).SetSpeed(speedRandom);

            return b3;
        }
    }



    private void OnTriggerEnter(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();
        if (pl)
        {
            if (!pl.Shielded)
                pl.CallFreeze(1);

            StopAllCoroutines();

            DestroyMe();
        }
        else if (coll.gameObject.layer == 9)
        {
            if (!_copy)
            {
                if (FloorRay() != default)
                {
                    var f = Instantiate(spawnFloor, FloorRay() + new Vector3(0, .1f, 0), transform.rotation);
                }
                Clone();
            }
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
