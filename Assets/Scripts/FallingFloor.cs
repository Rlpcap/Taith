using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFloor : MonoBehaviour
{
    public int ID;

    public float speed; //how fast it shakes
    public float amount; //how much it shakes

    public float fallingTime;
    public bool almostFalling = false;
    public bool timeStopped = false;

    Rigidbody _RB;

    private void Start()
    {
        _RB = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        if (almostFalling && !timeStopped)
            Shake();
    }

    public void Freeze()
    {
        timeStopped = true;
        _RB.velocity = Vector3.zero;
        _RB.constraints = RigidbodyConstraints.FreezeAll;
        _RB.isKinematic = true;
    }

    public void Unfreeze()
    {
        timeStopped = false;
        _RB.constraints = RigidbodyConstraints.None;
        _RB.isKinematic = false;
    }

    private void Shake()
    {
        _RB.transform.position = new Vector3(_RB.transform.position.x + Mathf.Sin(Time.time * speed + .5f) * amount, _RB.transform.position.y + Mathf.Sin(Time.time * speed - .5f) * amount, _RB.transform.position.z + Mathf.Sin(Time.time * speed) * amount);
    }

    public IEnumerator StartFalling()
    {
        almostFalling = true;
        yield return new WaitForSeconds(fallingTime);
        almostFalling = false;
        if(!timeStopped)
            _RB.isKinematic = false;
    }
}
