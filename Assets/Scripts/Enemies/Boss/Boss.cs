using MyFSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IUpdate
{
    public BossTimeBullet timeBulletPF;
    public BossEarthBullet earthBulletPF;

    public float xRandomDist, zRandomDist;
    public int attackAmount;
    Vector3[] _attackPlaces;

    EventFSM<int> _myFSM;
    int _fsmIndex = 0;
    event Action<Vector3> onShoot;

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
            onShoot += ShootTime;
        };

        time.FsmExit += (x) =>
        {
            onShoot -= ShootTime;
        };

        earth.FsmEnter += (x) =>
        {
            onShoot += ShootEarth;
        };

        earth.FsmExit += (x) =>
        {
            onShoot -= ShootEarth;
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

    void ShootTime(Vector3 pos)
    {
        var b = Instantiate(timeBulletPF, pos, Quaternion.identity);
    }

    void ShootEarth(Vector3 pos)
    {
        var b = Instantiate(earthBulletPF, pos, Quaternion.identity);
    }

    public void Attack(Vector3 pos)
    {
        for (int i = 0; i < attackAmount; i++)
        {
            float xRandom = UnityEngine.Random.Range(-xRandomDist, xRandomDist);
            float zRandom = UnityEngine.Random.Range(-zRandomDist, zRandomDist);
            var randomPlace = new Vector3(pos.x + xRandom, pos.y, pos.z + zRandom);
            _attackPlaces[i] = randomPlace;
        }

        //Empezar una courutina que haga los ataques en secuencia en vez de todos al instante?
        foreach (var place in _attackPlaces)
        {
            onShoot(place);
        }
    }

    public void Switch()
    {
        _fsmIndex++;
        _myFSM.SendInput(_fsmIndex);
    }

}
