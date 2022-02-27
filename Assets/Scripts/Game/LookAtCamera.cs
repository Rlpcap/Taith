using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour, ILateUpdate
{
    Camera _mainCam;

    public void Start()
    {
        UpdateManager.Instance.AddElementLateUpdate(this);
        _mainCam = Camera.main;
    }

    public void OnLateUpdate()
    {
        if(_mainCam)
            transform.LookAt(transform.position + _mainCam.transform.rotation * Vector3.forward, _mainCam.transform.rotation * Vector3.up);
    }
}
