using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IUpdate
{
    public int maxHP;
    int _currentHP;
    public FallingFloor standingPlatform;
    public float timeTillAction;
    protected bool _falling = false;

    protected Rigidbody _RB;
    protected PlayerModel _playerModel;
    public LayerMask playerMask;

    public virtual void Awake()
    {
        _RB = GetComponent<Rigidbody>();
    }

    public virtual void Start()
    {
        _playerModel = FindObjectOfType<PlayerModel>();
        UpdateManager.Instance.AddElementUpdate(this);
        StartCoroutine(ActiveAction(timeTillAction));
        _currentHP = maxHP;
    }

    public virtual void OnUpdate()
    {
        if (_falling)
            StopAllCoroutines();

        CheckFalling();
    }

    private void CheckFalling()
    {
        if (standingPlatform.Falling)
            _falling = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "MeleeCollider")
        {
            Debug.Log("Te pegué");
            _currentHP -= 5;
            if (_currentHP <= 0)
                OnDeath();
            other.gameObject.SetActive(false);
        }
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
