using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private Rigidbody _rb;
    public PlayerModel playerModel;
    public bool ItemAreaGrab;
    public LayerMask playerMask;


    public virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        playerModel = FindObjectOfType<PlayerModel>();
    }

    public virtual void Update()
    {
        ItemAreaGrab = Physics.CheckSphere(this.gameObject.transform.position, 5f, playerMask);

        if(ItemAreaGrab)
        {
            Action();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.gameObject.transform.position, 5f);
    }

    public abstract void Action();

}
