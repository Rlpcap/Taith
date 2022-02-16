using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour, IUpdate
{
    public float lifeTime;
    public float speed;
    public BossFloor spawnFloor;
    public GameObject alertObj;
    protected GameObject _myObj;
    protected float _prepareTime = 1;
    protected bool _started = false;

    protected virtual void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);
        StartCoroutine(Prepare(_prepareTime));
    }

    public virtual void OnUpdate()
    {
        Move();
    }

    void Move()
    {
        if(_started)
            transform.position += Vector3.down * speed * Time.deltaTime;
    }

    protected virtual IEnumerator Prepare(float t)
    {
        var mr = GetComponent<MeshRenderer>();
        var c = GetComponent<Collider>();

        mr.enabled = false;
        c.enabled = false;

        Ray ray = new Ray(transform.position, -Vector3.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, 1 << 9))
        {
            _myObj = Instantiate(alertObj, hit.point + new Vector3(0, 0.1f, 0), alertObj.transform.rotation);
        }

        yield return UpdateManager.WaitForSecondsCustom(t);

        mr.enabled = true;
        c.enabled = true;
        _started = true;
        StartCoroutine(Die(lifeTime));
    }

    protected IEnumerator Die(float t)
    {
        yield return UpdateManager.WaitForSecondsCustom(t);

        DestroyMe();
    }

    protected void DestroyMe()
    {
        Destroy(_myObj);
        UpdateManager.Instance.RemoveElementUpdate(this);
        Destroy(gameObject);
    }
}
