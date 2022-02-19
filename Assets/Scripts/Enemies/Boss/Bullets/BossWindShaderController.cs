using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWindShaderController : MonoBehaviour
{
    public float stepSpeed;
    float stepValue;

    public GameObject littleWind;
    Material windShader, littleWindShader;

    private void Awake()
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
        windShader.SetFloat("_forward", 1);
        littleWindShader.SetFloat("_forward", 1);

        stepValue = 0f;
        windShader.SetFloat("_step", stepValue);
        littleWindShader.SetFloat("_step", stepValue);
        while (stepValue > -0.28f)
        {

            stepValue -= stepSpeed;
            windShader.SetFloat("_step", stepValue);
            littleWindShader.SetFloat("_step", stepValue);
            yield return null;
        }
    }

    public IEnumerator FadeOutStepWind()
    {
        windShader.SetFloat("_forward", 0);
        littleWindShader.SetFloat("_forward", 0);

        stepValue = 0f;
        windShader.SetFloat("_step", stepValue);
        littleWindShader.SetFloat("_step", stepValue);
        while (stepValue > -0.28f)
        {

            stepValue -= stepSpeed;
            windShader.SetFloat("_step", stepValue);
            littleWindShader.SetFloat("_step", stepValue);
            yield return null;
        }
    }
}
