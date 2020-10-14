using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour, ILateUpdate
{
    GameObject _target;
    public float dampSpeed;

    public float distanceZ = 10;
    public float distanceY = 10;
    public float sensitivityX = 4;
    public float startingX;
    float currentX = 0;
    Renderer _lastHit;

    void Start()
    {
        UpdateManager.Instance.AddElementLateUpdate(this);
        _target = FindObjectOfType<PlayerModel>().gameObject;
        currentX = startingX;
    }

    public void OnLateUpdate()
    {
       Move();
       RotateCamera();
        BlockRaycast();
    }

    private void RotateCamera()
    {
        currentX += Input.GetAxis("Mouse X");
        transform.LookAt(_target.transform.position);
    }

    void Move()
    {
        Vector3 dir = new Vector3(0, distanceY, distanceZ);
        Quaternion rotation = Quaternion.Euler(0, currentX, 0);
        Vector3 nextPos = _target.transform.position + rotation * dir;

        //transform.position = Vector3.Lerp(transform.position, nextPos, dampSpeed);
        transform.position = nextPos;
    }

    void BlockRaycast()
    {
        GameObject overlapObject = null;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, _target.transform.position - transform.position, out hit))
        {
           // if (hit.collider.isTrigger) return;
            Debug.Log(hit.transform.gameObject.name);
            overlapObject = hit.transform.gameObject;
            if(overlapObject && overlapObject.GetComponent<Renderer>())
            {

                if (_lastHit != null && _lastHit != overlapObject.GetComponent<Renderer>())
                {
                    Debug.Log("ASD");
                    foreach (var item in _lastHit.materials)
                    {
                        item.SetFloat("_transparency", 1f);
                    }

                    _lastHit = overlapObject.GetComponent<Renderer>();

                    foreach (var item in _lastHit.materials)
                    {
                        item.SetFloat("_transparency", 0.3f);
                    }

                }else if (_lastHit == null || _lastHit != overlapObject.GetComponent<Renderer>())
                {
                    _lastHit = overlapObject.GetComponent<Renderer>();

                    foreach (var item in _lastHit.materials)
                    {
                        item.SetFloat("_transparency", 0.3f);
                    }
                }

            }
        }
        else
        {
                foreach (var item in _lastHit.materials)
                {
                    item.SetFloat("_transparency", 1f);
                }
        }
    }
}
