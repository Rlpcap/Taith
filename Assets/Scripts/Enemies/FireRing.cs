using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Experimental.PlayerLoop;

public class FireRing : MonoBehaviour, IUpdate
{
    float speed;

    float lifeTime;

    float setOnFireDuration;

    void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);
        StartCoroutine(DestroyGO(lifeTime));
    }

    public void OnUpdate()
    {
        Move();
    }

    void Move()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    IEnumerator DestroyGO(float time)
    {
        yield return new WaitForSeconds(time);
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

    //private void OnCollisionEnter(Collision coll)
    //{
    //    var pl = coll.gameObject.GetComponent<PlayerModel>();

    //    if (pl)
    //    {
    //        pl.SetOnFire(setOnFireDuration);
    //        StopAllCoroutines();
    //        UpdateManager.Instance.RemoveElementUpdate(this);
    //        Destroy(gameObject);
    //    }
    //}

    private void OnTriggerEnter(Collider coll)
    {
        Debug.Log("triggerie algo");
        var pl = coll.gameObject.GetComponent<PlayerModel>();

        if (pl)
        {
            pl.SetOnFire(setOnFireDuration);
            StopAllCoroutines();
            UpdateManager.Instance.RemoveElementUpdate(this);
            Destroy(gameObject);
        }
    }
}
