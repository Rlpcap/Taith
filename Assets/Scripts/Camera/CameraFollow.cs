using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour, ILateUpdate
{
    public GameObject target;
    public float dampSpeed;
    float zDistance;
    Vector3 dist;

    void Start()
    {
        UpdateManager.Instance.AddElementLateUpdate(this);
        zDistance = (target.transform.position - transform.position).z;
        dist = target.transform.position - transform.position;
    }

    public void OnLateUpdate()
    {
        Move();
       // RotateCamera();
    }

  /*  private void RotateCamera()
    {
        cameraPivot.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0));
    }*/

    void Move()
    {
        //var nextPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z - zDistance);
        var nextPos = new Vector3(target.transform.position.x, target.transform.position.y - dist.y, target.transform.position.z - dist.z);

        transform.position = Vector3.Lerp(transform.position, nextPos, dampSpeed);



    }
}
