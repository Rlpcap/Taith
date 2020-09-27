using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour, IUpdate, IFreezable
{
    public float freezeTime;
    public float speed;
    public float velocityLimit;
    public float jumpForce;
    public float dashForce;
    public float dashCD;
    public float dashDuration;
    public float timeStopRange;
    public Transform groundRayPosition;
    public GameObject meleeCollider;
    public CameraFollow cam;

    public delegate void CurrentPower();

    public CurrentPower currentPower;

    public float charDampTime;
    float dampSpeed;

    public LayerMask groundLayer;

    public float gravityForce;
    float _gravity = -9.81f;
    Vector3 _velocity;

    bool _onIce = false;
    public bool OnIce //Esto es sólo para poder acceder a la variable y modificarla desde afuera sin necesidad de tenerla pública.
    {
        get { return _onIce; }
        set { _onIce = value; }
    }
    bool _grounded;
    bool _canMove = true;
    bool _canDash = true;
    bool _isDashing = false;

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

    void Start()
    {
        _RB = GetComponent<Rigidbody>();
        _myController = new PlayerController(this, GetComponentInChildren<PlayerView>());
        UpdateManager.Instance.AddElementUpdate(this);
    }

    public void OnUpdate()
    {
        _myController.OnExecute();

        FloorCheck();

        if(!_isDashing)
            ApplyGravity();
    }

    public void Move(float x, float z, Vector3 dir)
    {
        if (_canMove)
        {
            Vector3 tempDir = (z * cam.transform.forward + x * cam.transform.right).normalized * speed;
            tempDir.y = _RB.velocity.y;
            if (!_onIce)
            {
                _RB.velocity = tempDir;
            }
            else
            {
                tempDir.y = 0;
                _RB.velocity += tempDir.normalized * speed * Time.deltaTime;
                _RB.velocity = Vector3.ClampMagnitude(_RB.velocity, velocityLimit);
            }

            if (dir != Vector3.zero)
            {
                float targetAngle = Mathf.Atan2(x, z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
                float dampedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref dampSpeed, charDampTime);
                transform.rotation = Quaternion.Euler(0, dampedAngle, 0);
            }
        }
        else
        {
            if (!_isDashing && !_onIce)
                _RB.velocity = Vector3.zero;
        }
    }

    void FloorCheck()
    {
        //var groundRay = Physics.Raycast(groundRayPosition.position, -Vector3.up, .3f, groundLayer);
        var groundSphere = Physics.CheckSphere(groundRayPosition.position, .4f, groundLayer);
        if (groundSphere)
            _grounded = true;
        else
            _grounded = false;
    }

    void ApplyGravity()
    {
        if (_grounded && _velocity.y < 0)
            _velocity.y = -2.5f;
        else
            _velocity.y += _gravity * Time.deltaTime;
        _RB.AddForce(_velocity * gravityForce * Time.deltaTime);
    }

    public void Jump()
    {
        if (_grounded && _canMove)
            _RB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void Dash()
    {
        if (_canDash)
        {
            StartCoroutine(UseDash());
            StartCoroutine(DashCooldown());
        }
    }

    public void TP(Vector3 newPos)
    {
        transform.position = newPos;
        _canTp = false;
    }

    public void UsePower()
    {
        if(currentPower!=null)
        {
            currentPower();
            currentPower = null;
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
    }

    public void Attack()
    {        
        if(!meleeCollider.gameObject.activeInHierarchy && _grounded)
            StartCoroutine(TurnCollider(0.2f));
    }

    IEnumerator UseDash()
    {
        _isDashing = true;
        _canMove = false;
        _velocity = Vector3.zero;
        _RB.velocity = transform.forward * dashForce;
        yield return new WaitForSeconds(dashDuration);
        _RB.velocity = Vector3.zero;
        _velocity.y = -5;
        _canMove = true;
        _isDashing = false;
    }

    IEnumerator DashCooldown()
    {
        _canDash = false;
        yield return new WaitForSeconds(dashCD);
        _canDash = true;
    }

    public IEnumerator TurnCollider(float t)
    {
        _canMove = false;
        meleeCollider.gameObject.SetActive(true);
        yield return new WaitForSeconds(t);
        _canMove = true;
        meleeCollider.gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider coll)
    {
        if (coll.gameObject.layer == 11)
            _onIce = true;
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.layer == 11)
            _onIce = false;
    }

    public void Freeze()
    {
        _canMove = false;
        foreach (var mat in GetComponent<MeshRenderer>().materials)
        {
            mat.color = Color.cyan;
        }
    }

    public void Unfreeze()
    {
        _canMove = true;
        foreach (var mat in GetComponent<MeshRenderer>().materials)
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
}
