using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWindEffect : MonoBehaviour
{
    BossWindShaderController _BWSC;

    void Start()
    {
        _BWSC = GetComponent<BossWindShaderController>();
        _BWSC.CallStepWind();
        StartCoroutine(Die(1.25f));
    }

    IEnumerator Die(float t)
    {
        yield return UpdateManager.WaitForSecondsCustom(t);
        yield return _BWSC.FadeOutStepWind();
        Destroy(gameObject);
    }
}
