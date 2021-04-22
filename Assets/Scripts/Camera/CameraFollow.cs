using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour, ILateUpdate
{
    public GameObject target;

    public CameraTarget cameraTarget;
    [Range(0f, 1f)] public float movementLerpSpeed = 0.2f;
    [Range(0f, 1f)] public float rotationLerpSpeed = 0.2f;

    Renderer _lastHit;


    void Start()
    {
        UpdateManager.Instance.AddElementLateUpdate(this);
    }

    public void OnLateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, cameraTarget.TargetPosition, movementLerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, cameraTarget.transform.rotation, rotationLerpSpeed);
        BlockRaycast();

    }

    void BlockRaycast()
    {
        GameObject overlapObject = null;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, target.transform.position - transform.position, out hit))
        {
            overlapObject = hit.transform.gameObject;
            if(overlapObject && overlapObject.GetComponentInChildren<Renderer>())
            {

                if (_lastHit != null && _lastHit != overlapObject.GetComponentInChildren<Renderer>())
                {
                    foreach (var item in _lastHit.materials)
                    {
                        item.SetFloat("_transparency", 1f);
                    }

                    _lastHit = overlapObject.GetComponentInChildren<Renderer>();

                    foreach (var item in _lastHit.materials)
                    {
                        item.SetFloat("_transparency", 0.3f);
                    }
                }
                else if (_lastHit == null || _lastHit == overlapObject.GetComponentInChildren<Renderer>())
                {
                    _lastHit = overlapObject.GetComponentInChildren<Renderer>();

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
