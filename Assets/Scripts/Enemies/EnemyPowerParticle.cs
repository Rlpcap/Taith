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
    Vector3 _offset;

    [SerializeField]
    float _speed;

    float _timer = 1;
    bool _moveToPlayer;

    void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);
        transform.position = transform.position + _offset;
    }

    public void OnUpdate()
    {

        Levitate();
        MoveTowardsPlayer(_moveToPlayer);
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

        if (_distance < 0.5f)
        {
            _player.GetPower(_powerAction, (int)_powerArt);
            Destroy(this.gameObject);
        }
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
