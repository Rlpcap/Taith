using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BossSubjects : MonoBehaviour
{
    public UIIndex myPower;
    public EnemyPowerParticle enemyPowerParticle;
    public float enemyPowerMoveSpeed;
    public Transform enemyPowerParticleSpawnPoint;
    public GameObject mesh;
    protected Action _myPowerAction = delegate { };
    protected PlayerModel _pl;
    Boss _boss;
    Animator _anim;

    protected virtual void Start()
    {
        _pl = FindObjectOfType<PlayerModel>();
        _boss = FindObjectOfType<Boss>();
        _anim = GetComponentInChildren<Animator>();
    }

    protected virtual void GetPower()
    {
        SpawnParticlePower(enemyPowerParticleSpawnPoint.position);
    }

    private void OnTriggerEnter(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();
        if (pl)
        {
            StartCoroutine(Die());
            GetPower();
        }
    }

    public void SpawnParticlePower(Vector3 position)
    {
        if (enemyPowerParticle != null)
        {
            var obj = Instantiate(enemyPowerParticle);
            obj.transform.position = position;
            obj.SetTarget(_pl).SetPowerArt(myPower).SetPowerAction(_myPowerAction).SetSpeed(enemyPowerMoveSpeed);
        }
    }

    IEnumerator Die()
    {
        float dissolveTime = 0;
        while (dissolveTime < 1)
        {
            dissolveTime += 0.008f;
            mesh.GetComponent<Renderer>().materials[0].SetFloat("_DissolveAmount", dissolveTime);
            mesh.GetComponent<Renderer>().materials[1].SetFloat("_DissolveAmount", dissolveTime);
            yield return null;
        }

        if(_boss)
            _boss.LooseHP();
        Destroy(gameObject);
    }
}
