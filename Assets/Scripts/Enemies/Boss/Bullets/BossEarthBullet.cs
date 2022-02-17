﻿using System.Collections;
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
        Ray ray = new Ray(transform.position, -Vector3.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, 1 << 9))
        {
            transform.position = hit.point + new Vector3(0, 4.5f, 0);
            //spawnear el barro
        }
        else
            DestroyMe();
    }
}
