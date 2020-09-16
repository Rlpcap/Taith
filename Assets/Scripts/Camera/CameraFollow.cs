using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour, ILateUpdate
{
    public GameObject target;
    public float dampSpeed;
    float zDistance;
    void Start()
    {
        UpdateManager.Instance.AddElementLateUpdate(this);
        zDistance = (target.transform.position - transform.position).z;
    }


    public void OnLateUpdate()
    {
        Move();
    }

    void Move()
    {
        var nextPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z - zDistance);
        transform.position = Vector3.Lerp(transform.position, nextPos, dampSpeed);
    }

}
