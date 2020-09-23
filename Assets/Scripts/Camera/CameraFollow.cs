using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour, ILateUpdate
{
    public GameObject target;
    public float dampSpeed;

    public float distanceZ = 10;
    public float distanceY = 10;
    public float sensitivityX = 4;
    float currentX = 0;

    void Start()
    {
        UpdateManager.Instance.AddElementLateUpdate(this);
    }

    public void OnLateUpdate()
    {
       Move();
       RotateCamera();
    }

    private void RotateCamera()
    {
        currentX += Input.GetAxis("Mouse X");
        transform.LookAt(target.transform.position);
    }

    void Move()
    {
<<<<<<< HEAD
        Vector3 dir = new Vector3(0, distanceY, distanceZ);
        Quaternion rotation = Quaternion.Euler(0, currentX, 0);
        Vector3 nextPos = target.transform.position + rotation * dir;
=======
        //var nextPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z - zDistance);
        var nextPos = new Vector3(target.transform.position.x-dist.x, target.transform.position.y - dist.y, target.transform.position.z - dist.z);

        transform.position = Vector3.Lerp(transform.position, nextPos, dampSpeed);


>>>>>>> fd9bc2aee2a2ec0de6ec28b0c653dfddd315ced6

        //transform.position = Vector3.Lerp(transform.position, nextPos, dampSpeed);
        transform.position = nextPos;
    }
}
