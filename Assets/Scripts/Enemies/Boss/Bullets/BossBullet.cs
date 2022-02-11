using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour, IUpdate
{
    public float lifeTime;
    public float speed;
    public BossFloor spawnFloor;

    protected virtual void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);
        StartCoroutine(Die(lifeTime));
    }

    public virtual void OnUpdate()
    {
        Move();
    }

    void Move()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
    }

    protected IEnumerator Die(float t)
    {
        yield return UpdateManager.WaitForSecondsCustom(t);

        UpdateManager.Instance.RemoveElementUpdate(this);
        Destroy(gameObject);
    }
}
