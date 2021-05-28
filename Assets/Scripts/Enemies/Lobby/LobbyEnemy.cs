using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyEnemy : MonoBehaviour
{
    protected PlayerModel _playerModel;
    public UIIndex myPower;
    public EnemyPowerParticle enemyPowerParticle;
    public Transform enemyPowerParticleSpawnPoint;
    protected Action _myPowerAction;
    Animator _anim;

    void Start()
    {
        _playerModel = FindObjectOfType<PlayerModel>();
        _anim = GetComponentInChildren<Animator>();
    }

    protected virtual void GetPower()
    {
        SpawnParticlePower(enemyPowerParticleSpawnPoint.position);
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.name == "MeleeCollider")
        {
            coll.gameObject.SetActive(false);
            _anim.SetTrigger("hit");
            GetPower();
        }
    }

    public void SpawnParticlePower(Vector3 position)
    {
        if (enemyPowerParticle != null)
        {
            var obj = Instantiate(enemyPowerParticle);
            obj.transform.position = position;
            obj.SetTarget(_playerModel).SetPowerArt(myPower).SetPowerAction(_myPowerAction);
        }
    }
}
