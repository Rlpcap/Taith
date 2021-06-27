using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Experimental.PlayerLoop;

public class FireRing : MonoBehaviour, IUpdate
{
    float speed;

    float lifeTime;

    float setOnFireDuration;

    public ParticleSystem fireRing;

    BoxCollider[] _boxColliders;

    AudioSource _AS;

    void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);
        StartCoroutine(DestroyGO(lifeTime));
        _boxColliders = GetComponents<BoxCollider>();
        _AS = GetComponent<AudioSource>();
    }

    public void OnUpdate()
    {
        Move();
    }

    void Move()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void Extinguish()
    {
        StopAllCoroutines();
        foreach (var collider in _boxColliders)
        {
            collider.enabled = false;
        }
        StartCoroutine(DestroyGO(0));
        //UpdateManager.Instance.RemoveElementUpdate(this);
        //Destroy(gameObject);
    }

    IEnumerator DestroyGO(float time)
    {
        yield return UpdateManager.WaitForSecondsCustom(time);
        //yield return new WaitForSeconds(time);
        fireRing.Stop();
        _AS.volume /= 2;
        yield return UpdateManager.WaitForSecondsCustom(.25f);
        _AS.volume /= 2;
        yield return UpdateManager.WaitForSecondsCustom(.25f);
        UpdateManager.Instance.RemoveElementUpdate(this);
        Destroy(gameObject);
    }

    public FireRing SetDuration(float dur)
    {
        lifeTime = dur;
        return this;
    }

    public FireRing SetSpeed(float spd)
    {
        speed = spd;
        return this;
    }

    public FireRing SetOnFireDuration(float dur)
    {
        setOnFireDuration = dur;
        return this;
    }

    private void OnTriggerEnter(Collider coll)
    {
        var pl = coll.gameObject.GetComponent<PlayerModel>();

        if (pl)
        {
            if(!pl.Shielded)
            pl.SetOnFire(setOnFireDuration);

            StopAllCoroutines();
            foreach (var collider in _boxColliders)
            {
                collider.enabled = false;
            }
            StartCoroutine(DestroyGO(0));
            //UpdateManager.Instance.RemoveElementUpdate(this);
            //Destroy(gameObject);
        }
    }
}
