using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEarthBullet : BossBullet
{
    protected override void Start()
    {
        base.Start();
        FloorCheck();
    }

    public override void OnUpdate()
    {
        Move();
    }

    void Move()
    {
        if(_started)
            transform.position += transform.forward * speed * Time.deltaTime;
    }

    protected override IEnumerator Prepare(float t)
    {
        yield return base.Prepare(t);
        Destroy(_myObj);
        var f = Instantiate(spawnFloor, transform.position - new Vector3(0, 4.4f, 0), transform.rotation);
    }

    void FloorCheck()
    {
        if(FloorRay() != default)
            transform.position = FloorRay() + new Vector3(0, 4.5f, 0);
        else
            DestroyMe();
    }
}
