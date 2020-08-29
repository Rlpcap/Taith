using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TempPlayer : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public LayerMask groundLayer;

    Rigidbody _RB;

    void Start()
    {
        _RB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            Move();
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
        if (Input.GetKeyDown(KeyCode.Q))
            StartCoroutine(StopTime());
    }

    private void Jump()
    {
        _RB.velocity = Vector3.zero;
        _RB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void Move()
    {
        _RB.transform.position += new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical")) * speed * Time.deltaTime;
    }

    IEnumerator StopTime()
    {
        var groundAround = Physics.OverlapSphere(transform.position, 7, groundLayer);
        foreach (var item in groundAround)
        {
            item.GetComponent<FallingFloor>().Freeze();
        }
        yield return new WaitForSeconds(5);
        foreach (var item in groundAround)
        {
            item.GetComponent<FallingFloor>().Unfreeze();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, 7);
    }
}
