using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : Enemy
{
    public float speed;
    public List<Transform> waypoints = new List<Transform>();
    protected int _index = 0;

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!_falling && !_isFreezed)
            Move();
    }

    private void Move()
    {
        if (Vector3.Distance(transform.position, waypoints[_index].position) < .3f)
        {
            _index++;
            if (_index > waypoints.Count - 1)
            {
                _index = 0;
            }
        }
        Vector3 dir = (waypoints[_index].position - transform.position).normalized;
        transform.forward = dir;
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public override void Action()
    {
    }

    public override void OnDeath()
    {
    }
}
