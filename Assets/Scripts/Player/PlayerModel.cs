using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour, IUpdate
{
    public float freezeTime;
    public float speed;
    public float jumpForce;
    public float timeStopRange;
    public Transform groundRayPosition;

    public LayerMask groundLayer;

    public float gravityForce;
    float _gravity = -9.81f;
    Vector3 _velocity;

    bool _grounded;

    bool _canTp = false;
    //Esto es sólo para poder acceder a la variable y modificarla desde afuera sin necesidad de tenerla pública.
    public bool CanTp
    {
        get
        {
            return _canTp;
        }
        set
        {
            _canTp = value;
        }
    }

    bool _canFreezeTime = false;
    //Esto es sólo para poder acceder a la variable y modificarla desde afuera sin necesidad de tenerla pública.
    public bool CanFreezeTime
    {
        get
        {
            return _canFreezeTime;
        }
        set
        {
            _canFreezeTime = value;
        }
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
        ApplyGravity();
    }

    public void Move(Vector3 dir)
    {
        _RB.transform.position += -dir * speed * Time.deltaTime;
    }

    void FloorCheck()
    {
        var groundRay = Physics.Raycast(groundRayPosition.position, -Vector3.up, .3f, groundLayer);
        var groundSphere = Physics.CheckSphere(groundRayPosition.position, .4f, groundLayer);
        if (groundRay)
            _grounded = true;
        else
            _grounded = false;
    }

    void ApplyGravity()
    {
        if (_grounded && _velocity.y < 0)
            _velocity.y = -2;
        _velocity.y += _gravity * Time.deltaTime;
        _RB.AddForce(_velocity * gravityForce * Time.deltaTime);
    }

    public void Jump()
    {
        if (_grounded)
        {
            _RB.velocity = new Vector3(_RB.velocity.x, 0, _RB.velocity.z);
            _RB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void TP(Vector3 newPos)
    {
        transform.position = newPos;
        _canTp = false;
    }

    public void StopTime()
    {
        if (_canFreezeTime)
        {
            _canFreezeTime = false;
            var propsInArea = Physics.OverlapSphere(transform.position, timeStopRange, groundLayer);
            foreach (var prop in propsInArea)
            {
                StartCoroutine(prop.GetComponent<IFreezable>().FreezeTime(freezeTime));
            }
        }
    }
}
