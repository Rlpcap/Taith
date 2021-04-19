﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerModel : MonoBehaviour, IUpdate, IFreezable
{
    public float freezeTime;
    public float velocityLimit;
    public float speed;
    public float mudSpeed;
    float _currentSpeed;
    public float jumpForce;
    public float dashForce;
    public float dashCD;
    public float dashDuration;
    public float timeStopRange;
    public float iceLaserLenght;
    public float iceSpellAngle;
    public float iceLaserDuration;
    public float earthShieldDuration;
    public int maxJumps;
    int _currentJumps;
    public Transform laserRayPos;
    public Transform groundRayPosition;
    public GameObject meleeCollider;
    public CameraTarget cam;

    Action _activePower;

    public float charDampTime;
    float _currentCharDampTime;
    float dampSpeed;

    public LayerMask enemyLayer;
    public LayerMask groundLayer;

    float _floorGravity;
    public float gravityForce;
    float _gravity = -9.81f;
    Vector3 _velocity;

    bool _onMud = false;
    bool _onFire = false;
    public bool OnFire { get { return _onFire; } set { _onFire = value; } }

    bool _onIce = false;
    public bool OnIce { get { return _onIce; } set { _onIce = value; } }

    bool _shielded = false;
    public bool Shielded { get { return _shielded; } set { _shielded = value; } }

    bool _checkGround;
    bool _grounded;
    bool _canMove = true;
    bool _canDash = true;
    bool _isDashing = false;
    bool _shootingLaser = false;
    bool _frozen = false;

    bool _canTp = false;    
    public bool CanTp //Esto es sólo para poder acceder a la variable y modificarla desde afuera sin necesidad de tenerla pública.
    {
        get { return _canTp; }
        set { _canTp = value; }
    }

    bool _canFreezeTime = false;    
    public bool CanFreezeTime //Esto es sólo para poder acceder a la variable y modificarla desde afuera sin necesidad de tenerla pública.
    {
        get { return _canFreezeTime; }
        set { _canFreezeTime = value; }
    }

    Rigidbody _RB;

    IController _myController;

    ClosestEnemy closestEnemy;

    public event Action<float> onShield = delegate { };
    public event Action<float> onFireDash = delegate { };
    public event Action<float> onLaser = delegate { };
    public event Action<float> onStopTime = delegate { };
    public event Action<int> onGetPower = delegate { };
    public event Action<float> onMove = delegate { };
    public event Action<float> onFire = delegate { };
    public event Action<bool> onJump = delegate { };
    public event Action onCast = delegate { };
    public event Action onAttack = delegate { };
    public event Action<bool> onCheckGround = delegate { };

    float timer = 1;

    void Start()
    {
        _RB = GetComponent<Rigidbody>();
        _myController = new PlayerController(this, GetComponentInChildren<PlayerView>());
        UpdateManager.Instance.AddElementUpdate(this);
        _currentSpeed = speed;
        _currentCharDampTime = charDampTime;
        _currentJumps = maxJumps;
        _checkGround = true;

        closestEnemy = new ClosestEnemy();

    }

    public void OnUpdate()
    {
        _myController.OnExecute();

        FloorCheck();
        Debug.Log(_checkGround);

        if (!_isDashing)
        {
            ApplyGravity();
        }

        if (_shootingLaser)
            CastIceRaycast();
    }

    public void Move(float x, float z)
    {
        if (_canMove)
        {
            onMove(Mathf.Abs(x) + Mathf.Abs(z));
            Vector3 tempDir = (z * Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized + x * cam.transform.right).normalized * _currentSpeed;
            tempDir.y = _RB.velocity.y;
            if (!_onIce)
            {
                _RB.velocity = tempDir;
            }
            else
            {
                tempDir.y = 0;
                _RB.velocity += tempDir.normalized * _currentSpeed * Time.deltaTime;
                _RB.velocity = Vector3.ClampMagnitude(_RB.velocity, velocityLimit);
            }

            if (x != 0 || z != 0)
            {
                float targetAngle = Mathf.Atan2(x, z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
                float dampedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref dampSpeed, _currentCharDampTime);
                transform.rotation = Quaternion.Euler(0, dampedAngle, 0);
            }
        }
        else
        {
            if (!_isDashing && !_onIce)
            {
                onMove(0);
                _RB.velocity = new Vector3(0, _RB.velocity.y, 0);
            }
        }
    }

    void FloorCheck()
    {
        if (_checkGround && !_onIce)
        {
            var ray = Physics.Raycast(groundRayPosition.position, Vector3.down, out var hit, .3f, groundLayer);
            if (ray && hit.collider.gameObject.GetComponent<FallingFloor>())
            {
                Debug.Log("hit");
                _RB.transform.position = new Vector3(_RB.transform.position.x, hit.point.y, _RB.transform.position.z);
                _RB.velocity = new Vector3(_RB.velocity.x, 0, _RB.velocity.z);
            }
        }

        var groundSphere = Physics.CheckSphere(groundRayPosition.position, .15f, groundLayer);
        if (groundSphere)
        {
            _grounded = true;

            if (!_onMud)
                _currentJumps = maxJumps;
            else
                _currentJumps = 0;
        }
        else
        {
            _grounded = false;
            if (_RB.velocity.y < 0)
                _checkGround = true;
        }

        onCheckGround(_grounded);
    }

    void ApplyGravity()
    {
        if (_grounded && _velocity.y < 0)
        {
            _velocity.y = -2.5f;
        }
        else
        {
            _velocity.y += _gravity * Time.deltaTime;
            _RB.AddForce(_velocity * gravityForce * Time.deltaTime);

        }


        transform.position += new Vector3(0, _floorGravity * Time.deltaTime, 0);
    }

    public void Jump()
    {
        if (_currentJumps > 0 && _canMove)
        {
            onJump(_grounded);

            _checkGround = false;
            //_velocity = Vector3.zero;
            _velocity.y = -2.5f;
            _RB.velocity = new Vector3(_RB.velocity.x, 0, _RB.velocity.z);
            _RB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _currentJumps--;
        }
    }

    public void Dash()
    {
        _checkGround = false;
        onFireDash(dashDuration);
        StartCoroutine(UseDash(dashDuration));
        //if (_canDash)
        //{
        //    StartCoroutine(UseDash());
        //    StartCoroutine(DashCooldown());
        //}
    }

    public void TP(Vector3 newPos)
    {
        transform.position = newPos;
        _canTp = false;
    }

    public void GetPower(Action power, int index)
    {
        onGetPower(index);
        _activePower = power;
    }

    public void UsePower()
    {
        if (_activePower != null && _canMove)
        {
            _activePower();
            _activePower = null;
            onCast();
        }
    }

    public void StopTime()
    {
        var propsInArea = Physics.OverlapSphere(transform.position, timeStopRange, groundLayer);
        foreach (var prop in propsInArea)
        {
            if (prop.GetComponent<IFreezable>() != null)
                StartCoroutine(prop.GetComponent<IFreezable>().FreezeTime(freezeTime));
        }
        onStopTime(freezeTime);
        //_activePower = null;              ACA SE LIMITA EL PODER!!!!!!
        //else
        //    _activePower = StopTime;
    }

    void CastIceRaycast()//Casteo el raycast que congela los enemigos
    {
        var hit = new RaycastHit();
        var ray = new Ray(laserRayPos.position, transform.forward);
        if (Physics.Raycast(ray, out hit, iceLaserLenght, 1 << 12))
        {
            if (!hit.collider.GetComponent<Enemy>().IsFrozen)
            {
                //StartCoroutine(hit.collider.GetComponent<Enemy>().FreezeTime(freezeTime));
                hit.collider.GetComponent<Enemy>().Freeze();
            }
        }
    }

    public void IceSpell()
    {
        onLaser(iceLaserDuration);

        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, iceLaserLenght, 1 << 12);

        foreach (var e in nearbyEnemies)
        {
            if (Vector3.Angle(transform.forward, e.transform.position) < iceSpellAngle)
                e.GetComponent<Enemy>().Freeze();
        }
    }

    public void IceLaser()
    {
        onLaser(iceLaserDuration);
        StartCoroutine(UseLaser(iceLaserDuration));//Inicio la courutina del laser
        //else
        //    _activePower = IceLaser;
    }

    public void SuperJump()
    {
        _checkGround = false;
        _currentJumps--;
        _velocity = Vector3.zero;
        _RB.velocity = new Vector3(_RB.velocity.x, 0, _RB.velocity.z);
        _RB.AddForce(Vector3.up * jumpForce * 3, ForceMode.Impulse);
        //_activePower = null;              ACA SE LIMITA EL PODER!!!!!!
        //else
        //    _activePower = SuperJump;
    }

    public void EarthShield()
    {
        onShield(earthShieldDuration);
        StartCoroutine(UseEarthShield(earthShieldDuration));
    }

    IEnumerator UseLaser(float f)//Manipulo un booleano, si esta en true se castea el raycast de hielo
    {
        //_activePower = null;              ACA SE LIMITA EL PODER!!!!!!
        _shootingLaser = true;
       // _currentSpeed /= 4;//Hago que el pj se mueva lento
       // _currentCharDampTime *= 4;//Hago que el pj rote lento
        yield return new WaitForSeconds(f);
        _shootingLaser = false;
       // _currentSpeed = speed;
       // _currentCharDampTime = charDampTime;
    }

    IEnumerator UseEarthShield(float time)
    {
        _shielded = true;
        yield return new WaitForSeconds(time);
        _shielded = false;
    }

    public void Attack()
    {        
        if(!meleeCollider.gameObject.activeInHierarchy && _grounded && !_frozen && _canMove)
        {
            Enemy enemy = closestEnemy.GetClosestEnemy(this);


            if (enemy)
            {
                float angle = Vector3.Angle(transform.forward, enemy.transform.position);

                if(angle<90f)
                {
                    Vector3 dir = enemy.transform.position - transform.position;
                    float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, targetAngle, 0);
                }
            }

            onAttack();
            StartCoroutine(TurnCollider(.75f));
        }
    }

    public void CallAttack()//Por si llamamos el ataque por evento
    {
        StartCoroutine(TurnCollider(0.2f));
    }

    IEnumerator UseDash(float time)
    {
        _isDashing = true;
        _canMove = false;
        _velocity = Vector3.zero;
        var dir = transform.forward + new Vector3(0, .1f, 0);
        _RB.velocity = dir * dashForce;
        yield return new WaitForSeconds(time);
        _RB.velocity = Vector3.zero;
        _velocity.y = -5;
        _canMove = true;
        _isDashing = false;
    }

    //IEnumerator DashCooldown()
    //{
    //    _canDash = false;
    //    yield return new WaitForSeconds(dashCD);
    //    _canDash = true;
    //}

    public IEnumerator EjectPlayer(float f, float t)
    {

        _RB.AddForce(new Vector3(f, f, f));
        _canMove = false;
        yield return new WaitForSeconds(t);
        _canMove = true;
    }

    public IEnumerator TurnCollider(float t)
    {
        _canMove = false;
        meleeCollider.gameObject.SetActive(true);
        yield return new WaitForSeconds(t);
        if(!_frozen)
            _canMove = true;
        meleeCollider.gameObject.SetActive(false);
    }

    public void SetOnFire(float duration)
    {
        onFire(duration);
        StartCoroutine(MoveRandom(duration));
    }

    IEnumerator MoveRandom(float time)
    {
        _canMove = false;
        //_frozen = true;
        //_onFire = true;
        _currentSpeed = 5;
        var remainingTime = time;
        int x = UnityEngine.Random.Range(-1, 2);
        int z = UnityEngine.Random.Range(-1, 2);

        if (x == 0)
            z = 1;

        while (remainingTime > time / 2)
        {
            //Move(x, z);
            _RB.transform.position += new Vector3(x, 0, z) * _currentSpeed * Time.deltaTime;

            onMove(Mathf.Abs(x) + Mathf.Abs(z));

            float targetAngle = Mathf.Atan2(x, z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            float dampedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref dampSpeed, _currentCharDampTime);
            transform.rotation = Quaternion.Euler(0, dampedAngle, 0);

            remainingTime -= Time.deltaTime;
            yield return null;
        }

        x = UnityEngine.Random.Range(-1, 2);
        z = UnityEngine.Random.Range(-1, 2);

        if (z == 0)
            x = -1;

        while (remainingTime > 0)
        {
            //Move(x, z);
            _RB.transform.position += new Vector3(x, 0, z) * _currentSpeed * Time.deltaTime;

            onMove(Mathf.Abs(x) + Mathf.Abs(z));

            float targetAngle = Mathf.Atan2(x, z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            float dampedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref dampSpeed, _currentCharDampTime);
            transform.rotation = Quaternion.Euler(0, dampedAngle, 0);

            remainingTime -= Time.deltaTime;
            yield return null;
        }

        _currentSpeed = speed;

        //_frozen = false;
        //_onFire = false;
        _canMove = true;
    }

    public void CallFreeze(float time)
    {
        StartCoroutine(FreezeTime(time));
    }

    public void Freeze()
    {
        _canMove = false;
        _frozen = true;
        foreach (var mat in GetComponentInChildren<SkinnedMeshRenderer>().materials)
        {
            mat.color = Color.cyan;
        }
    }

    public void Unfreeze()
    {
        _canMove = true;
        _frozen = false;
        foreach (var mat in GetComponentInChildren<SkinnedMeshRenderer>().materials)
        {
            mat.color = Color.white;
        }
    }

    public IEnumerator FreezeTime(float f)
    {
        Freeze();
        yield return new WaitForSeconds(f);
        Unfreeze();
    }

    private void OnTriggerStay(Collider coll)
    {
        if (coll.gameObject.layer == 11)
        {
            _onIce = true;
            _checkGround = false;
        }

        if (coll.gameObject.layer == 17)
        {
            _onMud = true;
            _currentSpeed = mudSpeed;
        }

        if (coll.gameObject.layer == 13)
        {
            var floor = coll.gameObject.GetComponentInParent<FallingFloor>();

            if(floor && floor.Falling)
            {
                _floorGravity = floor.gravity;
            }
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.layer == 11)
            _onIce = false;
        if (coll.gameObject.layer == 17)
        {
            _onMud = false;
            _currentSpeed = speed;
        }
        if (coll.gameObject.layer == 13)
            _floorGravity = 0;
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 10f);
    }
}
