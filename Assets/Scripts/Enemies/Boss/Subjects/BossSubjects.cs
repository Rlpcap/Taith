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
    bool _alive = true;

    protected virtual void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);
        _pl = FindObjectOfType<PlayerModel>();
        _boss = FindObjectOfType<Boss>();
        _anim = GetComponentInChildren<Animator>();
        bossBeam.transform.position = Vector3.zero;
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
        if (_currentRenderer && _alive)
        {
            _currentRenderer.SetPosition(0, _boss.transform.position + new Vector3(0, 5, 0));
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

        if(_boss)
            _boss.LooseHP();
    }

    public void CustomDestroy()
    {
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
}
