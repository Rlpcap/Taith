using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : Enemy
{
    public float speed;
    public float rotSpeed;
    public List<Transform> waypoints = new List<Transform>();
    protected int _index = 0;
    bool _canMove;

    public override void Start()
    {
        base.Start();
        _canMove = true;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!_falling && !_isFreezed && _canMove)
            Move();
    }

    private void Move()
    {
        if (Vector3.Distance(transform.position, waypoints[_index].position) < 1.5f)
        {
            _index++;
            if (_index > waypoints.Count - 1)
            {
                _index = 0;
            }
        }
        Vector3 dir = (waypoints[_index].position - transform.position).normalized;
        dir.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * rotSpeed);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public override void FeedbackAction()
    {
    }

    public override void Action()
    {
    }

    public override void OnDeath()
    {
    }

    public override void GetHitEffect()
    {
        StartCoroutine(CoolDoown(1f));
    }

    IEnumerator CoolDoown(float f)
    {
        _canMove = false;
        yield return new WaitForSeconds(f);
        _canMove = true;
    }
}
