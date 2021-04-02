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
    public LayerMask playerMask;
    public float shootRange;
    public UIIndex myPower;

    protected bool _falling = false;
    protected Rigidbody _RB;
    protected PlayerModel _playerModel;
    protected bool _isFreezed = false;
    protected Animator _anim;

    public bool canShoot;

    public bool IsFreezed
    {
        get { return _isFreezed; }
    }

    public virtual void Start()
    {
        _playerModel = FindObjectOfType<PlayerModel>();
        _anim = GetComponentInChildren<Animator>();
        UpdateManager.Instance.AddElementUpdate(this);
        //StartCoroutine(ActiveAction(prepareActionTime, doActionTime));
        _currentHP = maxHP;
        _RB = GetComponent<Rigidbody>();

    }

    public virtual void OnUpdate()
    {
        if (_falling)
        {
            StopAllCoroutines();
            if(_anim!=null)
                _anim.SetTrigger("goBackToIdle");
        }

        CheckFalling();

        canShoot = Vector3.Distance(transform.position, _playerModel.transform.position) < shootRange;
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

    //IEnumerator ActiveAction(float feedbackTime ,float actionTime)
    //{
    //    if (canShoot)
    //    {
    //        yield return new WaitForSeconds(feedbackTime);
    //        FeedbackAction();
    //        yield return new WaitForSeconds(actionTime);
    //        if (canShoot)
    //            Action();
    //        else if (_anim != null)
    //        {
    //            _anim.SetTrigger("goBackToIdle");
    //            _anim.SetBool("isRunning", false);
    //        }
    //    }
    //    yield return new WaitForSeconds(0.1f);
    //    StartCoroutine(ActiveAction(feedbackTime, actionTime));
    //}

    public abstract void FeedbackAction();
    public abstract void Action();
    public abstract void OnDeath();
    public abstract void GetHitEffect();

    public void Freeze()
    {
        _isFreezed = true;
        StopAllCoroutines();
        foreach (var mat in GetComponentInChildren<MeshRenderer>().materials)
        {
            mat.color = Color.cyan;
        }
    }

    public void Unfreeze()
    {
        _isFreezed = false;
        if (!_falling)
            Action();
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
