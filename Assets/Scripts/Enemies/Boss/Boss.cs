using MyFSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IUpdate
{
    public BossTimeBullet timeBulletPF;
    public BossEarthBullet earthBulletPF;
    public BossWindBullet windBulletPF;
    public BossIceBullet iceBulletPF;
    public BossFireBullet fireBulletPF;
    BossBullet _currentBullet;

    public float xRandomDist, zRandomDist;
    public int attackAmount;
    public float spawnRate;
    public GameObject redAlert;
    Vector3[] _attackPlaces;

    EventFSM<int> _myFSM;
    int _fsmIndex = 0;

    private void Awake()
    {
        var time = new State<int>("Time");
        var earth = new State<int>("Earth");
        var wind = new State<int>("Wind");
        var ice = new State<int>("Ice");
        var fire = new State<int>("Fire");
        var defeat = new State<int>("Defeat");

        #region StatesConfigurer
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

        time.FsmEnter += (x) =>
        {
            _currentBullet = timeBulletPF;
            //Mover al boss hacia la isla del tiempo
        };

        earth.FsmEnter += (x) =>
        {
            _currentBullet = earthBulletPF;
            //Mover al boss hacia la isla de la tierra
        };

        wind.FsmEnter += (x) =>
        {
            _currentBullet = windBulletPF;
            //Mover al boss hacia la isla del viento
        };

        ice.FsmEnter += (x) =>
        {
            _currentBullet = iceBulletPF;
            //Mover al boss hacia la isla del hielo
        };

        fire.FsmEnter += (x) =>
        {
            _currentBullet = fireBulletPF;
            //Mover al boss hacia la isla del fuego
        };

        defeat.FsmEnter += (x) =>
        {

        };

        _myFSM = new EventFSM<int>(time);
    }

    void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);
        _attackPlaces = new Vector3[attackAmount];        
    }

    public void OnUpdate()
    {
        _myFSM.OnUpdate();
    }

    void Shoot(Vector3 pos, Quaternion rot)
    {
        var b = Instantiate(_currentBullet, pos, rot);

        if (b.GetComponent<BossIceBullet>())
            b.GetComponent<BossIceBullet>().SetDir(Vector3.down);
    }

    public void Attack(Transform point)
    {
        for (int i = 0; i < attackAmount; i++)
        {
            float xRandom = UnityEngine.Random.Range(-xRandomDist, xRandomDist);
            float zRandom = UnityEngine.Random.Range(-zRandomDist, zRandomDist);
            var randomPlace = new Vector3(point.position.x + xRandom, point.position.y, point.position.z + zRandom);
            _attackPlaces[i] = randomPlace;
        }

        StartCoroutine(AttackRain(point.rotation));
    }

    IEnumerator AttackRain(Quaternion rot)
    {
        foreach (var place in _attackPlaces)
        {
            Shoot(place, rot);
            yield return UpdateManager.WaitForSecondsCustom(spawnRate);
        }
    }

    public void Switch()
    {
        _fsmIndex++;
        _myFSM.SendInput(_fsmIndex);
    }
}
