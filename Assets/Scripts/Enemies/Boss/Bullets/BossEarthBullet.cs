using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEarthBullet : MonoBehaviour, IUpdate
{
    public float speed;

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
        transform.position += Vector3.down * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider coll)
    {

    }
}
