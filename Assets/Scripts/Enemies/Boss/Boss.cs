using MyFSM;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour, IUpdate
{
    public Slider hpSlider;
    public int maxHP;
    int _currentHP;
    public Material iceEarthShader;
    public List<Transform> islandsWaypoints = new List<Transform>();
    public float moveSpeed;
    public float rotateSpeed;
    public float ySpeed;
    public float yOffset;
    public float scaleDownSpeed;
    public BossTimeBullet timeBulletPF;
    public BossEarthBullet earthBulletPF;
    public BossWindBullet windBulletPF;
    public BossIceBullet iceBulletPF;
    public BossFireBullet fireBulletPF;
    BossBullet _currentBullet;

    public float spawnRate;
    public PlayerModel pl;
    Vector3[] _attackPlaces;

    List<BossSubjects> _allSubjects = new List<BossSubjects>();

    EventFSM<int> _myFSM;
    int _fsmIndex = -1;

    bool _onLastStand = false;

    Animator _anim;

    private void Awake()
    {
        var idle = new State<int>("Idle");
        var time = new State<int>("Time");
        var earth = new State<int>("Earth");
        var wind = new State<int>("Wind");
        var ice = new State<int>("Ice");
        var fire = new State<int>("Fire");
        var lastStand = new State<int>("LastStand");
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
            .SetTransition(5, lastStand)
            .Done();

        StateConfigurer.Create(lastStand)
            .SetTransition(6, defeat)
            .Done();
        #endregion

        idle.FsmUpdate += PositionSelf;

        time.FsmEnter += (x) =>
        {
            StopAllCoroutines();
            InitializeCurrentSubjects();
            _currentBullet = timeBulletPF;
            StartCoroutine(MoveTowards(islandsWaypoints[_fsmIndex]));
        };
        time.FsmUpdate += PositionSelf;

        earth.FsmEnter += (x) =>
        {
            StopAllCoroutines();
            InitializeCurrentSubjects();
            _currentBullet = earthBulletPF;
            iceEarthShader.SetFloat("_IceMudLerp1", 1);
            StartCoroutine(MoveTowards(islandsWaypoints[_fsmIndex]));
        };
        earth.FsmUpdate += PositionSelf;

        wind.FsmEnter += (x) =>
        {
            StopAllCoroutines();
            InitializeCurrentSubjects();
            _currentBullet = windBulletPF;
            StartCoroutine(MoveTowards(islandsWaypoints[_fsmIndex]));
        };
        wind.FsmUpdate += PositionSelf;

        ice.FsmEnter += (x) =>
        {
            StopAllCoroutines();
            InitializeCurrentSubjects();
            _currentBullet = iceBulletPF;
            iceEarthShader.SetFloat("_IceMudLerp1", 0);
            StartCoroutine(MoveTowards(islandsWaypoints[_fsmIndex]));
        };
        ice.FsmUpdate += PositionSelf;

        fire.FsmEnter += (x) =>
        {
            StopAllCoroutines();
            InitializeCurrentSubjects();
            _currentBullet = fireBulletPF;
            StartCoroutine(MoveTowards(islandsWaypoints[_fsmIndex]));
        };
        fire.FsmUpdate += PositionSelf;

        lastStand.FsmEnter += (x) =>
        {
            StopAllCoroutines();
            InitializeCurrentSubjects();
            _onLastStand = true;
            _currentBullet = timeBulletPF;
            StartCoroutine(MoveTowards(islandsWaypoints[_fsmIndex]));
        };
        lastStand.FsmUpdate += PositionSelf;

        defeat.FsmEnter += (x) =>
        {
            Debug.Log("xP");
            StartCoroutine(FinalPosition(islandsWaypoints[_fsmIndex]));
        };

        _myFSM = new EventFSM<int>(idle);
    }

    void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);
        _anim = GetComponent<Animator>();
        _allSubjects = FindObjectsOfType<BossSubjects>().ToList();
        _currentHP = _allSubjects.Where(x => x.gameObject.activeInHierarchy).Count();
        UpdateHP();
        SceneRespawn();
    }

    public void OnUpdate()
    {
        _myFSM.OnUpdate();
    }

    void InitializeCurrentSubjects()
    {
        foreach (var subject in _allSubjects.Where(x => x.islandID == GameManager.Instance.BossLevelIndex))
        {
            subject.SetBoss(this);
            subject.InitializeSubject();
        }
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
        _anim.SetTrigger("cast");
        _attackPlaces = new Vector3[amount];
        for (int i = 0; i < amount; i++)
        {
            float tempX = UnityEngine.Random.Range(-xRandom, xRandom);
            float tempZ = UnityEngine.Random.Range(-zRandom, zRandom);
            var randomPlace = new Vector3(point.position.x + tempX, point.position.y, point.position.z + tempZ);
            randomPlace = RotatePos(randomPlace, point.position, point.rotation.eulerAngles);
            _attackPlaces[i] = randomPlace;
        }

        if(!_onLastStand)
            StartCoroutine(AttackRain(point.rotation, prepTime));
        else
            StartCoroutine(LastAttackRain(point.rotation, prepTime));
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

    IEnumerator LastAttackRain(Quaternion rot, float pt)
    {
        for (int i = 0; i < _attackPlaces.Length - 1; i++)
        {
            if (i < 6)
                _currentBullet = timeBulletPF;
            else if (i < 12)
                _currentBullet = earthBulletPF;
            else if (i < 18)
                _currentBullet = windBulletPF;
            else if (i < 24)
                _currentBullet = iceBulletPF;
            else
                _currentBullet = fireBulletPF;

            Shoot(_attackPlaces[i], rot, pt);
            yield return UpdateManager.WaitForSecondsCustom(spawnRate);
        }
    }

    IEnumerator MoveTowards(Transform point)
    {
        while(Vector3.Distance(transform.position, point.position) > .5f)
        {
            var endPos = new Vector3(point.position.x, transform.position.y, point.position.z);
            transform.position = Vector3.Lerp(transform.position, endPos, moveSpeed);
            yield return null;
        }
    }

    IEnumerator FinalPosition(Transform point)
    {
        while (Vector3.Distance(transform.position, point.position) > .5f)
        {
            transform.position = Vector3.Lerp(transform.position, point.position, moveSpeed / 4);
            transform.rotation = Quaternion.Lerp(transform.rotation, point.rotation, rotateSpeed / 4);
            if(transform.localScale.x > 20)
                transform.localScale = new Vector3(transform.localScale.x - scaleDownSpeed, transform.localScale.y - scaleDownSpeed, transform.localScale.z - scaleDownSpeed);
            yield return null;
        }
        while(Quaternion.Angle(transform.rotation, point.rotation) > .5f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, point.rotation, rotateSpeed / 4);
            if (transform.localScale.x > 20)
                transform.localScale = new Vector3(transform.localScale.x - scaleDownSpeed, transform.localScale.y - scaleDownSpeed, transform.localScale.z - scaleDownSpeed);
            yield return null;
        }
        while(transform.localScale.x > 20)
        {
            transform.localScale = new Vector3(transform.localScale.x - scaleDownSpeed, transform.localScale.y - scaleDownSpeed, transform.localScale.z - scaleDownSpeed);
            yield return null;
        }

        _anim.SetTrigger("defeat");
    }

    public void LooseHP()
    {
        _anim.SetTrigger("hit");
        _currentHP--;
        UpdateHP();
        if (_currentHP <= 0)
            LoosePower();
    }

    public void UpdateHP()
    {
        hpSlider.value = (float)_currentHP / maxHP;
    }

    void LoosePower()
    {
        _fsmIndex++;
        _myFSM.SendInput(_fsmIndex);
    }

    public void Switch()
    {
        GameManager.Instance.BossLevelIndex++;
        _fsmIndex++;
        _myFSM.SendInput(_fsmIndex);
    }

    void SceneRespawn()
    {
        _fsmIndex = GameManager.Instance.BossLevelIndex - 1;
        for (int i = 0; i < GameManager.Instance.BossLevelIndex; i++)
        {
            _myFSM.SendInput(i);
        }
        if(_myFSM.Current.Name == "Defeat")
        {
            transform.position = islandsWaypoints[_fsmIndex].position;
            transform.rotation = islandsWaypoints[_fsmIndex].rotation;
        }
    }
}
