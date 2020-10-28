using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IUpdate, IFreezable
{
    public int maxHP;
    int _currentHP;
    public FallingFloor standingPlatform;
    public float doActionTime;
    public float prepareActionTime;
    protected bool _falling = false;

    protected Rigidbody _RB;
    protected PlayerModel _playerModel;
    public LayerMask playerMask;
    protected bool _isFreezed = false;
    public bool IsFreezed
    {
        get { return _isFreezed; }
    }

    public virtual void Start()
    {
        _playerModel = FindObjectOfType<PlayerModel>();
        UpdateManager.Instance.AddElementUpdate(this);
        StartCoroutine(ActiveAction(prepareActionTime, doActionTime));
        _currentHP = maxHP;
        _RB = GetComponent<Rigidbody>();
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "MeleeCollider")
        {
            GetHitEffect();
            _currentHP -= 5;
            if (_currentHP <= 0)
                OnDeath();
            other.gameObject.SetActive(false);
        }
    }

    IEnumerator ActiveAction(float feedbackTime ,float actionTime)
    {
        yield return new WaitForSeconds(feedbackTime);
        FeedbackAction();
        yield return new WaitForSeconds(actionTime);
        Action();
        StartCoroutine(ActiveAction(feedbackTime, actionTime));
    }

    public abstract void FeedbackAction();
    public abstract void Action();
    public abstract void OnDeath();
    public abstract void GetHitEffect();

    public void Freeze()
    {
        _isFreezed = true;
        StopAllCoroutines();
        foreach (var mat in GetComponent<MeshRenderer>().materials)
        {
            mat.color = Color.cyan;
        }
    }

    public void Unfreeze()
    {
        _isFreezed = false;
        if (!_falling)
            StartCoroutine(ActiveAction(prepareActionTime, doActionTime));
        foreach (var mat in GetComponent<MeshRenderer>().materials)
        {
            mat.color = Color.white;
        }
    }

    public IEnumerator FreezeTime(float f)
    {
        Freeze();
        yield return new WaitForSeconds(f);
        if(this)
            Unfreeze();
    }
}
