using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IUpdate
{
    protected Rigidbody _rb;
    protected PlayerModel _playerModel;
    bool _itemAreaGrab;
    public LayerMask playerMask;

    public virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _playerModel = FindObjectOfType<PlayerModel>();
        UpdateManager.Instance.AddElementUpdate(this);
    }

    public void OnUpdate()
    {
        _itemAreaGrab = Physics.CheckSphere(transform.position, 5f, playerMask);

        if (_itemAreaGrab)
        {
            Action();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 5f);
    }

    public abstract void Action();

}
