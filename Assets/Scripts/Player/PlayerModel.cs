using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerModel : MonoBehaviour, IUpdate, IPause
{
    public float freezeTime;
    public float velocityLimit;
    public float speed;
    public float mudSpeed;
    float _currentSpeed;
    public float iceSpeedTest;
    Vector3 _storedRBVel;
    public float jumpForce;
    public float dashForce;
    public float dashCD;
    public float dashDuration;
    public float timeStopRange;
    public float iceLaserLenght;
    public float iceLaserDuration;
    public float earthShieldDuration;
    public int maxJumps;
    int _currentJumps;
    public Transform laserRayPos;
    public Transform groundRayPosition;
    public GameObject meleeCollider;
    public GameObject freezeCollider;
    public CameraTarget cam;
    public GameObject characterStaff;

    Action _activePower;

    public float charDampTime;
    float _currentCharDampTime;
    float dampSpeed;

    public LayerMask enemyLayer;
    public LayerMask groundLayer;

    float _floorGravity;
    public float gravityForce;
    float _currentGravityForce;
    public float CurrentGravityForce { get { return _currentGravityForce; } set { _currentGravityForce = value; } }

    float _gravity = -9.81f;
    Vector3 _velocity;

    public float hoverLaunchSpeed;
    public float hoverSpeed;
    public float hoverRadius;
    float _sinNumber;

    bool _onMud = false;
    public bool OnMud { get { return _onMud; } set { _onMud = value; } }
    bool _onFire = false;
    public bool OnFire { get { return _onFire; } set { _onFire = value; } }

    bool _onWind = false;
    public bool OnWind { get { return _onWind; } set { _onWind = value; } }

    bool _hovering = false;
    public bool Hovering { get { return _hovering; } set { _hovering = value; } }

    bool _onIce = false;
    public bool OnIce { get { return _onIce; } set { _onIce = value; } }
    bool _steppedOnIce = false;

    bool _shielded = false;
    public bool Shielded { get { return _shielded; } set { _shielded = value; } }

    bool _checkGround;
    bool _grounded;
    public bool Grounded { get { return _grounded; } }
    bool _canMove = true;

    bool _isDashing = false;
    public bool IsDashing { get { return _isDashing; } set { _isDashing = value; } }

    bool _shootingLaser = false;
    bool _frozen = false;
    bool _onWater = false;

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

    bool _onCoyoteTime;

    public static bool isLocked;

    Rigidbody _RB;

    IController _myController;

    ClosestEnemy closestEnemy;

    Interactable _interactingObject;
    bool _canInteract;

    public event Action<float> onShield = delegate { };
    public event Action<float> onFireDash = delegate { };
    public event Action<float> onLaser = delegate { };
    public event Action<float> onStopTime = delegate { };
    public event Action<int> onGetPower = delegate { };
    public event Action<float> onMove = delegate { };
    public event Action<float> onFire = delegate { };
    public event Action<float> onFreeze = delegate { };
    public event Action<float> onStoppedInTime = delegate { };
    public event Action<bool> onJump = delegate { };
    public event Action onMudJump = delegate { };
    public event Action onMudMove = delegate { };
    public event Action onCast = delegate { };
    public event Action onAttack = delegate { };
    public event Action<bool> onCheckGround = delegate { };
    public event Action<Color> onPortalTrigger = delegate { };
    public event Action onPausedGame = delegate { };
    public event Action onUnpausedGame = delegate { };
    public event Action onInteractableEnter = delegate { };
    public event Action onInteractableExit = delegate { };
    public event Action onInteract = delegate { };
    public event Action onWaterEnter = delegate { };
    public event Action onWaterExit = delegate { };

    float timer = 1;
    public PlayerView playerView;

    void Start()
    {
        playerView = GetComponent<PlayerView>();
        _RB = GetComponent<Rigidbody>();
        _myController = new PlayerController(this, GetComponentInChildren<PlayerView>());
        UpdateManager.Instance.AddElementUpdate(this);
        UpdateManager.Instance.AddElementPausable(this);
        _currentSpeed = speed;
        _currentCharDampTime = charDampTime;
        _currentJumps = maxJumps;
        _currentGravityForce = gravityForce;
        _checkGround = true;

        closestEnemy = new ClosestEnemy();

    }

    public void OnUpdate()
    {
        if (!isLocked)
            _myController.OnExecute();

        FloorCheck();

        Hover();

        if (!_isDashing)
        {
            ApplyGravity();
        }

    }

    public void Move(float x, float z)
    {
        if (_canMove)
        {
            onMove(Mathf.Abs(x) + Mathf.Abs(z));
            Vector3 tempDir = (z * Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized + x * cam.transform.right).normalized * _currentSpeed;
            tempDir.y = _RB.velocity.y;
            if (!_steppedOnIce)
            {
                _RB.velocity = tempDir;
            }
            else
            {
                tempDir.y = 0;
                _RB.velocity += tempDir.normalized /** _currentSpeed*/ * iceSpeedTest * Time.deltaTime;

                float mag = Mathf.Sqrt(_RB.velocity.x * _RB.velocity.x + _RB.velocity.z * _RB.velocity.z);
                if (mag > velocityLimit)
                {
                    float clampX = (_RB.velocity.x / mag) * velocityLimit;
                    float clampZ = (_RB.velocity.z / mag) * velocityLimit;
                    Vector3 clampedVel = new Vector3(clampX, _RB.velocity.y, clampZ);
                    _RB.velocity = clampedVel;
                }
            }

            if (x != 0 || z != 0)
            {
                float targetAngle = Mathf.Atan2(x, z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
                float dampedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref dampSpeed, _currentCharDampTime);
                transform.rotation = Quaternion.Euler(0, dampedAngle, 0);
                if (_onMud)
                {
                    SoundManager.PlaySound(SoundManager.Sound.MudStep, transform.position);
                    onMudMove();

                }
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
        if (_checkGround && !_onWind && !_steppedOnIce)
        {
            var ray = Physics.Raycast(groundRayPosition.position, Vector3.down, out var hit, .3f, groundLayer);
            if (ray)
            {
                _RB.transform.position = new Vector3(_RB.transform.position.x, hit.point.y, _RB.transform.position.z);
                _RB.velocity = new Vector3(_RB.velocity.x, 0, _RB.velocity.z);
            }
        }

        var groundSphere = Physics.CheckSphere(groundRayPosition.position, .3f, groundLayer);
        if (groundSphere)
        {
            _onCoyoteTime = false;

            _grounded = true;
            if (!_onIce)
                _steppedOnIce = false;
            else
                _steppedOnIce = true;

            _currentJumps = maxJumps;
        }
        else
        {
            if (_RB.velocity.y < 0)
                _checkGround = true;
            if (!_onCoyoteTime)
                StartCoroutine(CoyoteTime());
        }

        onCheckGround(_grounded);
    }

    IEnumerator CoyoteTime()
    {
        _onCoyoteTime = true;
        yield return UpdateManager.WaitForSecondsCustom(.1f);
        _currentJumps = maxJumps;
        _grounded = false;
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
            _RB.AddForce(_velocity * _currentGravityForce * Time.deltaTime);

        }


        transform.position += new Vector3(0, _floorGravity * Time.deltaTime, 0);
    }

    public void Jump()
    {
        if (_currentJumps > 0 && _canMove && !_onMud)
        {
            onJump(_grounded);
            SoundManager.PlaySound(SoundManager.Sound.PlayerJump);

            _checkGround = false;
            //_velocity = Vector3.zero;
            _velocity.y = -2.5f;
            _RB.velocity = new Vector3(_RB.velocity.x, 0, _RB.velocity.z);
            _RB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _currentJumps--;
        }
        else if (_onMud)
        {
            onMudJump();
            SoundManager.PlaySound(SoundManager.Sound.MudStep, transform.position);
        }
    }

    public void ObjectJump()
    {
        onJump(_grounded);
        //_currentJumps = 1;
        _checkGround = false;
        _velocity.y = -2.5f;
        _RB.velocity = new Vector3(_RB.velocity.x, 0, _RB.velocity.z);
        _RB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void PumpkinJump()
    {
        onJump(_grounded);
        //_currentJumps = 1;
        _checkGround = false;
        _velocity.y = -2.5f;
        _RB.velocity = new Vector3(_RB.velocity.x, 0, _RB.velocity.z);
        _RB.AddForce(Vector3.up * jumpForce * 2, ForceMode.Impulse);
    }

    public void Dash()
    {
        _checkGround = false;
        onFireDash(dashDuration);
        StartCoroutine(UseDash(dashDuration));
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

    public void IceSpell()
    {
        onLaser(iceLaserDuration);

        StartCoroutine(IceBubbleSpawn(iceLaserDuration));

        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, iceLaserLenght, 1 << 12);

        foreach (var e in nearbyEnemies)
        {
            var enemy = e.GetComponent<Enemy>();
            var fire = e.GetComponent<FireRing>();
            if (enemy)
                enemy.Freeze();
            else if (fire)
            {
                fire.Extinguish();
            }
        }
    }

    IEnumerator IceBubbleSpawn(float time)
    {
        var bubble = Instantiate(freezeCollider, transform.position, Quaternion.identity);

        yield return UpdateManager.WaitForSecondsCustom(time);

        Destroy(bubble);
        _onIce = false;
    }

    public void SuperJump()
    {
        _checkGround = false;
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

    IEnumerator UseEarthShield(float time)
    {
        _shielded = true;
        yield return UpdateManager.WaitForSecondsCustom(time);
        //yield return new WaitForSeconds(time);
        _shielded = false;
    }

    public void Interact()
    {
        if (_canInteract && _interactingObject != null)
        {
            _interactingObject.Interact();
            onInteract();
        }
    }

    public void Attack()
    {
        if (!meleeCollider.gameObject.activeInHierarchy && _grounded && !_frozen && _canMove && !_onWater)
        {
            if (_onIce)
                _RB.velocity /= 2.5f;

            Enemy enemy = closestEnemy.GetClosestEnemy(this);


            if (enemy)
            {
                float angle = Vector3.Angle(transform.forward, enemy.transform.position);

                if (angle < 90f)
                {
                    Vector3 dir = enemy.transform.position - transform.position;
                    float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, targetAngle, 0);
                }
            }

            onAttack();
            SoundManager.PlaySound(SoundManager.Sound.PlayerAttack, transform.position);
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

        yield return UpdateManager.WaitForSecondsCustom(time);

        _RB.velocity = Vector3.zero;
        _velocity.y = -5;
        _canMove = true;
        _isDashing = false;
    }

    public IEnumerator EjectPlayer(float f, float t)
    {

        _RB.AddForce(new Vector3(f, f, f));
        _canMove = false;
        yield return UpdateManager.WaitForSecondsCustom(t);
        _canMove = true;
    }

    public IEnumerator TurnCollider(float t)
    {
        _canMove = false;
        meleeCollider.gameObject.SetActive(true);

        yield return UpdateManager.WaitForSecondsCustom(t);

        if (!_frozen)
            _canMove = true;
        meleeCollider.gameObject.SetActive(false);
    }

    public void SendHovering(float duration, Transform tornado)
    {
        StartCoroutine(HoveringTimer(duration, tornado));
    }

    IEnumerator HoveringTimer(float t, Transform pos)
    {
        var upAmmount = transform.position.y + 3;

        _canMove = false;
        _checkGround = false;
        _RB.isKinematic = true;

        while(Vector3.Distance(transform.position, pos.position) > 0.5f)
        {
            var dir = (pos.position - transform.position).normalized;
            transform.position += dir * hoverLaunchSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        while (transform.position.y < upAmmount)
        {
            transform.position += Vector3.up * hoverLaunchSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _hovering = true;

        yield return UpdateManager.WaitForSecondsCustom(t);

        _hovering = false;
        _RB.isKinematic = false;
        _checkGround = true;
        _canMove = true;
        _sinNumber = 0;
    }

    void Hover()
    {
        if (_hovering)
        {
            _sinNumber += hoverSpeed * Time.deltaTime;

            var offset = new Vector3(0, Mathf.Cos(_sinNumber), 0) * hoverRadius;

            transform.position += offset;
        }
    }

    public void SetOnFire(float duration)
    {
        _onFire = true;
        onFire(duration);
        StartCoroutine(MoveRandom(duration));
    }

    IEnumerator MoveRandom(float time)
    {
        _canMove = false;
        _currentSpeed = 5;
        var remainingTime = time;
        int x = UnityEngine.Random.Range(-1, 2);
        int z = UnityEngine.Random.Range(-1, 2);

        if (x == 0)
            z = 1;

        while (remainingTime > time / 2)
        {
            if (!UpdateManager.GamePaused)//**Asegurarse de que funcione**
            {
                _RB.transform.position += new Vector3(x, 0, z) * _currentSpeed * Time.deltaTime;

                //onMove(Mathf.Abs(x) + Mathf.Abs(z));

                float targetAngle = Mathf.Atan2(x, z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
                float dampedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref dampSpeed, _currentCharDampTime);
                transform.rotation = Quaternion.Euler(0, dampedAngle, 0);

                remainingTime -= Time.deltaTime;
            }

            yield return null;
        }

        x = UnityEngine.Random.Range(-1, 2);
        z = UnityEngine.Random.Range(-1, 2);

        if (z == 0)
            x = -1;

        while (remainingTime > 0)
        {
            if (!UpdateManager.GamePaused)//**Asegurarse de que funcione**
            {
                _RB.transform.position += new Vector3(x, 0, z) * _currentSpeed * Time.deltaTime;

                //onMove(Mathf.Abs(x) + Mathf.Abs(z));

                float targetAngle = Mathf.Atan2(x, z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
                float dampedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref dampSpeed, _currentCharDampTime);
                transform.rotation = Quaternion.Euler(0, dampedAngle, 0);

                remainingTime -= Time.deltaTime;
            }
            yield return null;
        }

        _currentSpeed = speed;

        _canMove = true;
        _onFire = false;
    }

    public void CallStopInTime(float time)
    {
        StartCoroutine(StopInTime(time));
    }

    IEnumerator StopInTime(float t)
    {
        onStoppedInTime(t);
        Freeze();
        yield return UpdateManager.WaitForSecondsCustom(t);
        Unfreeze();
    }

    public void Freeze()
    {
        _canMove = false;
        _frozen = true;
    }

    public void Unfreeze()
    {
        _canMove = true;
        _frozen = false;
    }

    public void CallFreeze(float time)
    {
        StartCoroutine(FreezeTime(time));
    }

    public IEnumerator FreezeTime(float f)
    {
        onFreeze(f);
        Freeze();
        yield return UpdateManager.WaitForSecondsCustom(f);
        Unfreeze();
    }

    public void UnMud()
    {
        _onMud = false;
        _currentSpeed = speed;
    }

    public void OnWaterEnter()
    {
        _onWater = true;
        onWaterEnter();
    }

    public void OnWaterExit()
    {
        _onWater = false;
        onWaterExit();
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

            if (floor && floor.Falling)
            {
                _floorGravity = floor.gravity;
            }
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.layer == 19)
        {
            var interactableObj = coll.transform.parent.GetComponent<Interactable>();
            if (interactableObj)
            {
                _interactingObject = interactableObj;
                _canInteract = true;
                onInteractableEnter();
            }
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.layer == 11)
        {
            _onIce = false;
        }
        if (coll.gameObject.layer == 17)
            UnMud();
        if (coll.gameObject.layer == 13)
            _floorGravity = 0;
        if (coll.gameObject.layer == 19)
        {
            var interactableObj = coll.transform.parent.GetComponent<Interactable>();
            if (interactableObj)
            {
                if (_interactingObject)
                {
                    _interactingObject.EndInteraction();
                    onInteractableExit();
                }
                _interactingObject = null;
                _canInteract = false;
            }
        }
    }

    public void OnPortalTrigger(Color tc)
    {
        //isLocked = true;
        //UpdateManager.Instance.RemoveElementUpdate(this);
        onPortalTrigger(tc);
    }

    public void OnPause()
    {
        _storedRBVel = _RB.velocity;
        _RB.isKinematic = true;
        onPausedGame();
    }

    public void OnUnpause()
    {
        _RB.isKinematic = false;
        _RB.velocity = _storedRBVel;
        onUnpausedGame();
    }

    public void OnPauseBook()
    {

    }

    public void OnUnPauseBook()
    {

    }
}
