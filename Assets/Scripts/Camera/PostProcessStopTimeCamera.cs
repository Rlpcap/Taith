using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class PostProcessStopTimeCamera : MonoBehaviour,IUpdate
{

    // Post Process Material
    public Material PostProcessMat;
    public LayerMask stopTimeMask;

    // Function called by Unity after all rendering is complete to render image
    // Here we copy the render source image (source) to the final render image (destination) applying out mat (PostProcessMat)
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, PostProcessMat);
    }
    void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 1);
    }

    public void OnUpdate()
    {
        bool isOn = Physics.CheckSphere(transform.position, 1, stopTimeMask);
        if(isOn)
        {
            PostProcessMat.SetFloat("_alpha", 1);
        }
        else
        {
            PostProcessMat.SetFloat("_alpha", 0);
        }
        Debug.Log(isOn);
    }
}
