using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemPlatform : FallingFloor
{
    public float speed;
    public Transform moveUpPoint, moveDownPoint;
    bool _inactive = true;
    public bool Inactive { get { return _inactive; } set { _inactive = value; } }

    private void Update()
    {
        if (!timeStopped)
        {
            if (_inactive)
                MoveDown();
            else
                MoveUp();
        }
    }

    public void MoveUp()
    {
        transform.position = Vector3.Lerp(transform.position, moveUpPoint.position, Time.deltaTime * speed);
    }

    public void MoveDown()
    {
        transform.position = Vector3.Lerp(transform.position, moveDownPoint.position, Time.deltaTime * speed);
    }
}
