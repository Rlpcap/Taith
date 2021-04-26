using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemPlatform : FallingFloor
{
    public float moveSpeed;
    public Transform moveUpPoint;
    Vector3 moveDownPos;
    bool _inactive = true;
    public bool Inactive { get { return _inactive; } set { _inactive = value; } }

    public override void Start()
    {
        base.Start();
        moveDownPos = transform.position;
    }

    private void Update()
    {
        if (!timeStopped && !_falling)
        {
            if (_inactive)
                MoveDown();
            else
                MoveUp();
        }
    }

    public void MoveUp()
    {
        transform.position = Vector3.Lerp(transform.position, moveUpPoint.position, Time.deltaTime * moveSpeed);
    }

    public void MoveDown()
    {
        transform.position = Vector3.Lerp(transform.position, moveDownPos, Time.deltaTime * moveSpeed);
    }
}
