using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BossSubjects : MonoBehaviour, IUpdate
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
    public List<GameObject> remainingChains = new List<GameObject>();
    public int islandID;
    public LineRenderer bossBeam;
    LineRenderer _currentRenderer;
    public float beamReduceSpeed;
    bool _alive = true;
    bool beamDestroyed = false;

    protected virtual void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);
        _pl = FindObjectOfType<PlayerModel>();
        _anim = GetComponentInChildren<Animator>();
    }

    public void OnUpdate()
    {
        UpdateBeamPos();
    }

    public void InitializeSubject()
    {
        _currentRenderer = Instantiate(bossBeam, Vector3.zero, Quaternion.identity);
    }

    void UpdateBeamPos()
    {
        if (_currentRenderer)
        {
            _currentRenderer.SetPosition(0, _boss.transform.position + new Vector3(0, 5, 0));
            if(_alive)
                _currentRenderer.SetPosition(1, transform.position + new Vector3(0, 3, 0));
        }
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
            StartCoroutine(ReduceBeam());
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

    IEnumerator ReduceBeam()
    {
        var dist = Vector3.Distance(_currentRenderer.GetPosition(0), _currentRenderer.GetPosition(1));
        while (Vector3.Distance(_currentRenderer.GetPosition(0), _currentRenderer.GetPosition(1)) > 1f)
        {
            var dir = (_currentRenderer.GetPosition(1) - _currentRenderer.GetPosition(0)).normalized;
            _currentRenderer.SetPosition(1, _currentRenderer.GetPosition(1) - dir * beamReduceSpeed * dist);
            yield return null;
        }
        _boss.LooseHP();
        Destroy(_currentRenderer.gameObject);
        beamDestroyed = true;
    }

    IEnumerator Die()
    {
        _alive = false;
        GetComponent<Collider>().enabled = false;
        _anim.SetTrigger("death");
        float dissolveTime = 0;
        while (dissolveTime < 1)
        {
            dissolveTime += 0.012f;
            foreach (var mat in mesh.GetComponent<Renderer>().materials)
            {
                mat.SetFloat("_DissolveAmount", dissolveTime);
            }
            yield return null;
        }
    }

    public void CustomDestroy()
    {
        StopAllCoroutines();
        if (!beamDestroyed)
        {
            _boss.LooseHP();
            Destroy(_currentRenderer.gameObject);
        }
        SetChainsParent();
        UpdateManager.Instance.RemoveElementUpdate(this);
        Destroy(gameObject);
    }

    void SetChainsParent()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, 5, 0), -Vector3.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, 1 << 9))
        {
            foreach (var go in remainingChains)
            {
                go.transform.parent = hit.transform;
            }
        }
        else
        {
            foreach (var go in remainingChains)
            {
                go.transform.parent = transform.parent;
            }
        }
    }

    public BossSubjects SetBoss(Boss b)
    {
        _boss = b;
        return this;
    }
}
