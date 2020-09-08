using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IUpdate
{
    public int maxHP;
    int _currentHP;
    public FallingFloor standingPlatform;
    public float speed;
    public float timeTillAction;
    public List<Transform> waypoints = new List<Transform>();
    protected int _index = 0;
    bool _falling = false;

    protected Rigidbody _RB;
    protected PlayerModel _playerModel;
    bool _itemAreaGrab;
    public LayerMask playerMask;

    public virtual void Awake()
    {
        _RB = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _playerModel = FindObjectOfType<PlayerModel>();
        UpdateManager.Instance.AddElementUpdate(this);
        StartCoroutine(ActiveAction(timeTillAction));
    }

    public void OnUpdate()
    {
        _itemAreaGrab = Physics.CheckSphere(transform.position, 5f, playerMask);

        if (_itemAreaGrab)
        {
            OnDeath();
        }
        if(!_falling)
            Move();
        CheckFalling();
    }

    private void CheckFalling()
    {
        if (!standingPlatform.GetComponent<Rigidbody>().isKinematic)
            _falling = true;
    }

    private void Move()
    {
        if(Vector3.Distance(transform.position, waypoints[_index].position) < .3f)
        {
            _index++;
            if(_index > waypoints.Count - 1)
            {
                _index = 0;
            }
        }
        Vector3 dir = (waypoints[_index].position - transform.position).normalized;
        transform.forward = dir;
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 5f);
    }

    IEnumerator ActiveAction(float t)
    {
        yield return new WaitForSeconds(t);
        Action();
        StartCoroutine(ActiveAction(t));
    }

    public abstract void Action();
    public abstract void OnDeath();

}
