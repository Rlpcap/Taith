using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPowerParticle : MonoBehaviour, IUpdate
{
    PlayerModel _player;

    UIIndex _powerArt;

    Action _powerAction;

    [SerializeField]
    float _speed;

    [SerializeField]
    float _rotateSpeed;

    [SerializeField]
    float _timer = .5f;
    bool _moveToPlayer;

    Vector3 _spinAxis = Vector3.up;

    void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);
        _moveToPlayer = true;
    }

    public void OnUpdate()
    {
        //Levitate();
        MoveTowardsPlayer(_moveToPlayer);
        //EndSpin(_moveToPlayer);
    }

    private void Levitate()
    {
        if (_timer < 0)
            return;

        _timer -= Time.deltaTime;

        if (_timer < 0)
            _moveToPlayer = true;
    }

    private void MoveTowardsPlayer(bool moveToPlayer)
    {
        if (!moveToPlayer)
            return;

        Vector3 _dir = (_player.characterStaff.transform.position - transform.position).normalized;
        float _distance = Vector3.Distance(transform.position, _player.characterStaff.transform.position);

        transform.position += _dir * _speed * Time.deltaTime;

        if (_distance < 1f)
        {
            _player.GetPower(_powerAction, (int)_powerArt);
            Destroy(this.gameObject);
            //_moveToPlayer = false;
            //StartCoroutine(DelayDestroy());
        }
    }

    void EndSpin(bool notSpinning)
    {
        if (notSpinning)
            return;
        transform.RotateAround(_player.characterStaff.transform.position, _spinAxis, _rotateSpeed * Time.deltaTime);
        transform.position = new Vector3(_player.characterStaff.transform.position.x, _player.characterStaff.transform.position.y, _player.characterStaff.transform.position.z);
    }

    IEnumerator DelayDestroy()
    {
        yield return UpdateManager.WaitForSecondsCustom(2f);
        //yield return new WaitForSeconds(2f);
        _player.GetPower(_powerAction, (int)_powerArt);
        Destroy(this.gameObject);
    }

    public EnemyPowerParticle SetTarget(PlayerModel pl)
    {
        _player = pl;
        return this;
    }

    public EnemyPowerParticle SetPowerArt(UIIndex p)
    {
        _powerArt = p;
        return this;
    }

    public EnemyPowerParticle SetPowerAction(Action a)
    {
        _powerAction = a;
        return this;
    }

    void OnDestroy()
    {
        UpdateManager.Instance.RemoveElementUpdate(this);
    }
}
