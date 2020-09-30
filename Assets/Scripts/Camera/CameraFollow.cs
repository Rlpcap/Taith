using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour, ILateUpdate
{
    GameObject _target;
    public float dampSpeed;

    public float distanceZ = 10;
    public float distanceY = 10;
    public float sensitivityX = 4;
    float currentX = 0;

    void Start()
    {
        UpdateManager.Instance.AddElementLateUpdate(this);
        _target = FindObjectOfType<PlayerModel>().gameObject;
    }

    public void OnLateUpdate()
    {
       Move();
       RotateCamera();
    }

    private void RotateCamera()
    {
        currentX += Input.GetAxis("Mouse X");
        transform.LookAt(_target.transform.position);
    }

    void Move()
    {
        Vector3 dir = new Vector3(0, distanceY, distanceZ);
        Quaternion rotation = Quaternion.Euler(0, currentX, 0);
        Vector3 nextPos = _target.transform.position + rotation * dir;

        //transform.position = Vector3.Lerp(transform.position, nextPos, dampSpeed);
        transform.position = nextPos;
    }
}
