using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindShaderController : MonoBehaviour
{
    public float stepSpeed;
    float stepValue;

    Material shader;

    void Start()
    {
        shader = GetComponent<MeshRenderer>().material;
    }

    public void CallStepWind()
    {
        StartCoroutine(StepWind());
    }

    IEnumerator StepWind()
    {
        shader.SetFloat("_step", 0f);
        while (stepValue < 1)
        {
            stepValue += stepSpeed;
            shader.SetFloat("_step", stepValue);
            yield return null;
        }
    }
}
