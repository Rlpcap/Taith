using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthWall : MonoBehaviour, IUpdate
{
    public float speed;

    public float lifeTime;
    public float hitDuration;

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

    private void OnCollisionEnter(Collision coll)
    {
        var player = coll.gameObject.GetComponent<PlayerModel>();
        if (player)
        {
            StopAllCoroutines();
            StartCoroutine(DestroyGO(hitDuration));
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
}
