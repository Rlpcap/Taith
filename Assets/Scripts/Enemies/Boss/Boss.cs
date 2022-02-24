using MyFSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IUpdate
{
    public int maxHP;
    int _currentHP;
    public Material iceEarthShader;
    public List<Transform> islandsWaypoints = new List<Transform>();
    public float moveSpeed;
    public float rotateSpeed;
    public float ySpeed;
    public float yOffset;
    public BossTimeBullet timeBulletPF;
    public BossEarthBullet earthBulletPF;
    public BossWindBullet windBulletPF;
    public BossIceBullet iceBulletPF;
    public BossFireBullet fireBulletPF;
    BossBullet _currentBullet;

    public float spawnRate;
    public PlayerModel pl;
    Vector3[] _attackPlaces;

    EventFSM<int> _myFSM;
    int _fsmIndex = -1;

    private void Awake()
    {
        var idle = new State<int>("Idle");
        var time = new State<int>("Time");
        var earth = new State<int>("Earth");
        var wind = new State<int>("Wind");
        var ice = new State<int>("Ice");
        var fire = new State<int>("Fire");
        var defeat = new State<int>("Defeat");

        #region StatesConfigurer
        StateConfigurer.Create(idle)
            .SetTransition(0, time)
            .Done();

        StateConfigurer.Create(time)
            .SetTransition(1, earth)
            .Done();

        StateConfigurer.Create(earth)
            .SetTransition(2, wind)
            .Done();

        StateConfigurer.Create(wind)
            .SetTransition(3, ice)
            .Done();

        StateConfigurer.Create(ice)
            .SetTransition(4, fire)
            .Done();

        StateConfigurer.Create(fire)
            .SetTransition(5, defeat)
            .Done();
        #endregion

        idle.FsmUpdate += PositionSelf;

        time.FsmEnter += (x) =>
        {
            _currentBullet = timeBulletPF;
            StartCoroutine(MoveTowards(islandsWaypoints[0]));
        };
        time.FsmUpdate += PositionSelf;

        earth.FsmEnter += (x) =>
        {
            _currentBullet = earthBulletPF;
            iceEarthShader.SetFloat("_IceMudLerp1", 1);
            StartCoroutine(MoveTowards(islandsWaypoints[1]));
        };
        earth.FsmUpdate += PositionSelf;

        wind.FsmEnter += (x) =>
        {
            _currentBullet = windBulletPF;
            StartCoroutine(MoveTowards(islandsWaypoints[2]));
        };
        wind.FsmUpdate += PositionSelf;

        ice.FsmEnter += (x) =>
        {
            _currentBullet = iceBulletPF;
            iceEarthShader.SetFloat("_IceMudLerp1", 0);
            StartCoroutine(MoveTowards(islandsWaypoints[3]));
        };
        ice.FsmUpdate += PositionSelf;

        fire.FsmEnter += (x) =>
        {
            _currentBullet = fireBulletPF;
            StartCoroutine(MoveTowards(islandsWaypoints[4]));
        };
        fire.FsmUpdate += PositionSelf;

        defeat.FsmEnter += (x) =>
        {

        };

        _myFSM = new EventFSM<int>(idle);
    }

    void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);
        _currentHP = maxHP;
    }

    public void OnUpdate()
    {
        _myFSM.OnUpdate();
    }

    void PositionSelf()
    {
        var newPos = new Vector3(transform.position.x, pl.transform.position.y + yOffset, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPos, ySpeed);
        var dir = (pl.transform.position - transform.position).normalized;
        dir.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, dir, rotateSpeed);
    }

    void Shoot(Vector3 pos, Quaternion rot, float t)
    {
        var b = Instantiate(_currentBullet, pos, rot);
        b.SetPrepareTime(t);
    }

    public void Attack(Transform point, int amount, float prepTime, float xRandom, float zRandom)
    {
        _attackPlaces = new Vector3[amount];
        for (int i = 0; i < amount; i++)
        {
            float tempX = UnityEngine.Random.Range(-xRandom, xRandom);
            float tempZ = UnityEngine.Random.Range(-zRandom, zRandom);
            var randomPlace = new Vector3(point.position.x + tempX, point.position.y, point.position.z + tempZ);
            randomPlace = RotatePos(randomPlace, point.position, point.rotation.eulerAngles);
            _attackPlaces[i] = randomPlace;
        }

        StartCoroutine(AttackRain(point.rotation, prepTime));
    }

    Vector3 RotatePos(Vector3 rotatingObj, Vector3 pivot, Vector3 angles)
    {
        var dir = rotatingObj - pivot;
        dir = Quaternion.Euler(angles) * dir;
        rotatingObj = dir + pivot;
        return rotatingObj;
    }

    IEnumerator AttackRain(Quaternion rot, float pt)
    {
        foreach (var place in _attackPlaces)
        {
            Shoot(place, rot, pt);
            yield return UpdateManager.WaitForSecondsCustom(spawnRate);
        }
    }

    IEnumerator MoveTowards(Transform point)
    {
        while(Vector3.Distance(transform.position, point.position) > .5f)
        {
            var endPos = new Vector3(point.position.x, transform.position.y, point.position.z);
            transform.position = Vector3.Lerp(transform.position, endPos, moveSpeed);
            //transform.rotation = Quaternion.Lerp(transform.rotation, point.rotation, rotateSpeed);
            yield return null;
        }
        //while(Quaternion.Angle(transform.rotation, point.rotation) > .5f)
        //{
        //    transform.rotation = Quaternion.Lerp(transform.rotation, point.rotation, rotateSpeed);
        //    yield return null;
        //}
    }

    public void LooseHP()
    {
        _currentHP--;
        if (_currentHP <= 0)
            LoosePower();
    }

    void LoosePower()
    {
        Debug.Log("Quedé débil >:c");
    }

    public void Switch()
    {
        _fsmIndex++;
        _myFSM.SendInput(_fsmIndex);
    }
}
