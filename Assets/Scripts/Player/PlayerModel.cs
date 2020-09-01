using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour, IUpdate
{
    public float freezeTime;
    public float speed;
    public float jumpForce;
    public LayerMask groundLayer;
    public bool canTp;

    bool _timeSkill = false;

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
    }

    public void Move(Vector3 dir)
    {
        _RB.transform.position += -dir * speed * Time.deltaTime;
    }

    public void Jump()
    {
        _RB.velocity = Vector3.zero;
        _RB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void StopTime()
    {
        var propsInArea = Physics.OverlapSphere(transform.position, 7, groundLayer);
        foreach (var prop in propsInArea)
        {
            StartCoroutine(prop.GetComponent<IFreezable>().FreezeTime(freezeTime));            
        }
        //_timeSkill = false;
        //if (_timeSkill)
        //{
        //}
    }
}
