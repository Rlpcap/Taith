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
        transform.position += transform.forward * speed * Time.deltaTime;
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
        {
            UpdateManager.Instance.RemoveElementUpdate(this);
            Destroy(gameObject);
        }    
    }
}
