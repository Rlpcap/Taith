using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBullet : MonoBehaviour, IUpdate
{
    public float speed;
    public float plFreezeTime;

    public LayerMask playerLayer;

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
        if (coll.GetComponent<PlayerModel>())
            StartCoroutine(coll.GetComponent<PlayerModel>().FreezeTime(plFreezeTime));
        //if (coll.gameObject != ignoreObject)
        //{
        //    UpdateManager.Instance.RemoveElementUpdate(this);
        //    Destroy(gameObject);
        //}
    }
}
