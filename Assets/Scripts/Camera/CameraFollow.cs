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
    public float yHeight;

    float currentX = 0;
    float currentY = 0;

    Renderer _lastHit;

    Quaternion rotation;

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
        ZoomCamera();

    }

    private void ZoomCamera()
    {
        float moveSpeed = 2f;
        RaycastHit hit;
        if (Physics.Raycast(_target.transform.position, (transform.position - _target.transform.position).normalized, out hit))
        {
            if (hit.collider.gameObject.layer == 9)
            {
                Debug.Log(hit.collider.name);

                distanceY = Mathf.Lerp(distanceY, hit.distance, moveSpeed * Time.deltaTime);
                distanceZ = Mathf.Lerp(distanceZ, hit.distance, moveSpeed * Time.deltaTime);

                distanceZ = Mathf.Clamp(distanceZ, 1, 10);
                distanceY = Mathf.Clamp(distanceY, 1, 10);

                Debug.Log(hit.distance);

            }
            else
            {
                distanceY = Mathf.Lerp(distanceY, 10, moveSpeed * Time.deltaTime);
                distanceZ = Mathf.Lerp(distanceZ, 10, moveSpeed * Time.deltaTime);

                distanceZ = Mathf.Clamp(distanceZ, 1, 10);
                distanceY = Mathf.Clamp(distanceY, 1, 10);
                Debug.Log(hit.distance);
            }
        }

    }

    private void RotateCamera()
    {
        currentX += Input.GetAxis("Mouse X");
        currentY += Input.GetAxis("Mouse Y");
        transform.LookAt(_target.transform.position + new Vector3(0, yHeight, 0));
    }

    void Move()
    {
        Vector3 dir = new Vector3(0, distanceY, distanceZ);
        //Quaternion rotation = Quaternion.Euler(0, currentX, 0);//Bloqueada
        currentY = Mathf.Clamp(currentY, -20, 80);
        rotation = Quaternion.Euler(currentY, currentX, 0);//Freelook
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
            overlapObject = hit.transform.gameObject;
            if(overlapObject && overlapObject.GetComponent<Renderer>())
            {

                if (_lastHit != null && _lastHit != overlapObject.GetComponent<Renderer>())
                {
                    foreach (var item in _lastHit.materials)
                    {
                        item.SetFloat("_transparency", 1f);
                    }

                    _lastHit = overlapObject.GetComponent<Renderer>();

                    foreach (var item in _lastHit.materials)
                    {
                        item.SetFloat("_transparency", 0.3f);
                    }
                }
                else if (_lastHit == null || _lastHit == overlapObject.GetComponent<Renderer>())
                {
                    _lastHit = overlapObject.GetComponent<Renderer>();

                    foreach (var item in _lastHit.materials)
                    {
                        item.SetFloat("_transparency", 0.3f);
                    }
                }
            }
            else if(_lastHit != null)
            {
                foreach (var item in _lastHit.materials)
                {
                    item.SetFloat("_transparency", 1f);
                }
            }
        }
    }
}
