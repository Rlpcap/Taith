using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRing : MonoBehaviour, IUpdate
{
    public float speed;

    public float lifeTime;

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

}
