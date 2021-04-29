using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindShaderController : MonoBehaviour
{
    public float stepSpeed;
    float stepValue;

    public GameObject littleWind;
    Material windShader, littleWindShader;

    void Start()
    {
        windShader = GetComponent<Renderer>().materials[0];
        littleWindShader = littleWind.GetComponent<Renderer>().materials[0];
    }

    public void CallStepWind()
    {
        StartCoroutine(StepWind());
    }

    IEnumerator StepWind()
    {
        stepValue = 0f;
        windShader.SetFloat("_step", stepValue);
        littleWindShader.SetFloat("_step", stepValue);
        while (stepValue > -1)
        {
            stepValue -= stepSpeed;
            windShader.SetFloat("_step", stepValue);
            littleWindShader.SetFloat("_step", stepValue);
            yield return null;
        }
    }
}
