using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindShaderController : MonoBehaviour, IUpdate
{
    public float stepSpeed;
    float stepValue;

    public GameObject littleWind;
    Material windShader, littleWindShader;

    WindBullet _parent;

    private void Awake()
    {
        windShader = GetComponent<Renderer>().materials[0];
        littleWindShader = littleWind.GetComponent<Renderer>().materials[0];
        _parent = transform.parent.GetComponent<WindBullet>();
    }

    void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);
        //windShader = GetComponent<Renderer>().materials[0];
        //littleWindShader = littleWind.GetComponent<Renderer>().materials[0];
    }

    public void OnUpdate()
    {
        if (_parent.CollidingWithGround && windShader.GetFloat("_step") < -0.13f)
        {
            windShader.SetFloat("_step", -0.14f);
            littleWindShader.SetFloat("_step", -0.14f);
        }
        else if(windShader.GetFloat("_step") < -0.13f)
        {
            windShader.SetFloat("_step", -0.28f);
            littleWindShader.SetFloat("_step", -0.28f);
        }
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
            if (_parent.CollidingWithGround && windShader.GetFloat("_step") < -0.13f) break;

            stepValue -= stepSpeed;
            windShader.SetFloat("_step", stepValue);
            littleWindShader.SetFloat("_step", stepValue);
            yield return null;
        }
    }

    public void StopStepWind(GameObject w)
    {
        StartCoroutine(FadeOutStepWind(w));
    }

    IEnumerator FadeOutStepWind(GameObject wind)
    {
        windShader.SetFloat("_forward", 0);
        littleWindShader.SetFloat("_forward", 0);

        stepValue = 0f;
        windShader.SetFloat("_step", stepValue);
        littleWindShader.SetFloat("_step", stepValue);
        while (stepValue > -0.28f)
        {
            if (_parent.CollidingWithGround) break;

            stepValue -= stepSpeed;
            windShader.SetFloat("_step", stepValue);
            littleWindShader.SetFloat("_step", stepValue);
            yield return null;
        }

        wind.SetActive(false);
    }
}
