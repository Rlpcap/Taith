using MyFSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IUpdate, IFreezable
{
    public int maxHP;
    protected int _currentHP;
    public FallingFloor standingPlatform;
    public float doActionTime;
    public float prepareActionTime;
    protected float stunnedTime = .75f;
    public LayerMask playerMask;
    public UIIndex myPower;

    protected bool _falling = false;
    protected Rigidbody _RB;
    protected PlayerModel _playerModel;
    protected bool _isFrozen = false;
    protected Animator _anim;

    protected bool _isDead = false;

    public bool IsFrozen
    {
        get { return _isFrozen; }
    }


    protected EventFSM<string> _myFSM;
    protected State<string> normal, special, hit, falling, die;

    public virtual void Start()
    {
        _playerModel = FindObjectOfType<PlayerModel>();
        _anim = GetComponentInChildren<Animator>();
        UpdateManager.Instance.AddElementUpdate(this);
        _currentHP = maxHP;
        _RB = GetComponent<Rigidbody>();

        normal = new State<string>("Normal");
        special = new State<string>("Special");
        hit = new State<string>("Hit");
        falling = new State<string>("Falling");
        die = new State<string>("Die");

        StateConfigurer.Create(normal)
            .SetTransition("special", special)
            .SetTransition("hit", hit)
            .SetTransition("falling", falling)
            .Done();

        StateConfigurer.Create(special)
            .SetTransition("normal", normal)
            .SetTransition("hit", hit)
            .SetTransition("falling", falling)
            .Done();

        StateConfigurer.Create(hit)
            .SetTransition("normal", normal)
            .SetTransition("special", special)
            .SetTransition("falling", falling)
            .SetTransition("hit", hit)
            .SetTransition("die", die)
            .Done();

        StateConfigurer.Create(falling)
            .SetTransition("hit", hit)
            .Done();

        normal.FsmUpdate += () =>
        {
            CheckFalling();

            if (_falling)
                SendInputToFSM("falling");
        };

        special.FsmUpdate += () =>
        {
            CheckFalling();

            if (_falling)
                SendInputToFSM("falling");
        };

        hit.FsmEnter += x =>
        {
            GetHitEffect();
            _currentHP -= 5;
            if (_currentHP <= 0)
                SendInputToFSM("die");
            else
                StartCoroutine(DelayedSendInputToFsm(stunnedTime, "normal"));
        };

        falling.FsmEnter += x =>
        {
            StopAllCoroutines();
        };

        die.FsmEnter += x =>
        {
            _isDead = true;
            OnDeath();
        };

        _myFSM = new EventFSM<string>(normal);
    }

    public virtual void OnUpdate()
    {
        //if (_falling)
        //{
        //    StopAllCoroutines();
        //    //if(_anim!=null)
        //    //    _anim.SetTrigger("goBackToIdle");
        //}

        //CheckFalling();

        //_myFSM.OnUpdate();
    }

    protected void SendInputToFSM(string input)
    {
        _myFSM.SendInput(input);
    }

    protected IEnumerator DelayedSendInputToFsm(float time, string input)
    {
        yield return new WaitForSeconds(time);
        SendInputToFSM(input);
    }

    protected void CheckFalling()
    {
        if (standingPlatform.Falling)
            _falling = true;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "MeleeCollider" && !_isDead)
        {
            other.gameObject.SetActive(false);
            SendInputToFSM("hit");
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
        _isFrozen = true;
        StopAllCoroutines();
        foreach (var mat in GetComponentInChildren<MeshRenderer>().materials)
        {
            mat.color = Color.cyan;
        }
    }

    public void Unfreeze()
    {
        _isFrozen = false;
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
